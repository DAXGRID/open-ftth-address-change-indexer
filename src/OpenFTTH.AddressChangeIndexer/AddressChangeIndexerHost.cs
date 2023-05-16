using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenFTTH.EventSourcing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed class AddressChangeIndexerHost : BackgroundService
{
    const int BULK_COUNT_MAX = 50_000;
    const int BULK_INSERT_DELAY_MS = 250;
    const int CATCHUP_TIME_MS = 600_000;
    private readonly ILogger<AddressChangeIndexerHost> _logger;
    private readonly IEventStore _eventStore;
    private readonly IDatabaseAddressChangeIndex _databaseAddressChangeIndex;

    public AddressChangeIndexerHost(
        ILogger<AddressChangeIndexerHost> logger,
        IEventStore eventStore,
        IDatabaseAddressChangeIndex databaseAddressChangeIndex)
    {
        _logger = logger;
        _eventStore = eventStore;
        _databaseAddressChangeIndex = databaseAddressChangeIndex;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {HostName}.", nameof(AddressChangeIndexerHost));

        _logger.LogInformation("Creating schema if it does not already exist.");
        _databaseAddressChangeIndex.InitSchema();

        var addressChangeProjection = _eventStore.Projections.Get<AddressChangeProjection>();
        var addressChangesReaderCh = addressChangeProjection.AddressChanges;

        var dehydrateTask = Task.Run(async () =>
        {
            _logger.LogInformation("Starting dehydration.");
            await _eventStore
                .DehydrateProjectionsAsync(stoppingToken)
                .ConfigureAwait(false);

            _logger.LogInformation(
                "Finished dehydration, memory after dehydration {MibiBytes}.",
                Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(CATCHUP_TIME_MS, stoppingToken).ConfigureAwait(false);
                _logger.LogInformation("Checking for new events.");
                await _eventStore
                    .CatchUpAsync(stoppingToken)
                    .ConfigureAwait(false);
            }
        }, stoppingToken);

        var processAddressChangesTask = Task.Run(async () =>
        {
            _logger.LogInformation("Starting address change processor.");

            var addressChangesBulk = new BlockingCollection<AddressChange>(
                new ConcurrentBag<AddressChange>(),
                BULK_COUNT_MAX);

            var addressChangesReader = addressChangesReaderCh
                .ReadAllAsync(stoppingToken)
                .ConfigureAwait(false);

            var readerChannelTask = Task.Run(async () =>
            {
                await foreach (var addressChange in addressChangesReader)
                {
                    addressChangesBulk.Add(addressChange);
                }
            }, stoppingToken);

            var bulkInsertTask = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // We need to make sure we always use the same count.
                    var changesCount = addressChangesBulk.Count;
                    if (changesCount > 0)
                    {
                        var bulkInsertChanges = new AddressChange[changesCount];
                        for (var i = 0; i < changesCount; i++)
                        {
                            bulkInsertChanges[i] = addressChangesBulk.Take();
                        }

                        try
                        {
                            _databaseAddressChangeIndex.BulkInsert(bulkInsertChanges);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("{Exception}", ex);
                            throw;
                        }

                        _logger.LogInformation(
                            "Finished bulk inserting {Count} address changes.",
                            changesCount);
                    }

                    await Task.Delay(BULK_INSERT_DELAY_MS).ConfigureAwait(false);
                }
            }, stoppingToken);

            await Task.WhenAll(readerChannelTask, bulkInsertTask).ConfigureAwait(false);
        }, stoppingToken);

        try
        {
            await Task.WhenAll(dehydrateTask, processAddressChangesTask).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Do nothing we do not care when operation is canceled,
            // it will always be because of a shutdown request from OS.
            _logger.LogInformation("OperationCanceledException, requested shutdown...");
        }
        catch (AggregateException)
        {
            throw;
        }
    }
}
