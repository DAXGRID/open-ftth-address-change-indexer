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
        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

        _logger.LogInformation("Starting {HostName}.", nameof(AddressChangeIndexerHost));

        _logger.LogInformation("Creating schema if it does not already exist.");
        _databaseAddressChangeIndex.InitSchema();

        var addressChangeProjection = _eventStore.Projections.Get<AddressChangeProjection>();
        var addressChangesReaderCh = addressChangeProjection.AddressChanges;

        var dehydrateTask = Task.Run(async () =>
        {
            try
            {
                _logger.LogInformation("Starting dehydration.");
                await _eventStore
                    .DehydrateProjectionsAsync(cancellationTokenSource.Token)
                    .ConfigureAwait(false);

                _logger.LogInformation(
                    "Finished dehydration, memory after dehydration {MibiBytes}.",
                    Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024);

                using var _ = File.Create("/tmp/healthy");

                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await Task.Delay(CATCHUP_TIME_MS, cancellationTokenSource.Token).ConfigureAwait(false);
                    _logger.LogInformation("Checking for new events.");
                    await _eventStore
                        .CatchUpAsync(cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("{Exception}", ex);
                await cancellationTokenSource.CancelAsync().ConfigureAwait(false);
                throw;
            }
        }, cancellationTokenSource.Token);

        var processAddressChangesTask = Task.Run(async () =>
        {
            var lastRunSequenceNumber = _databaseAddressChangeIndex.HighestSequenceNumber();
            _logger.LogInformation(
                "Starting address change processor, the last runs max sequence number was {HighestSequenceNumber}, will only procecss all sequence numbers after.",
                lastRunSequenceNumber);

            var addressChangesBulk = new BlockingCollection<AddressChange>(
                new ConcurrentBag<AddressChange>(),
                BULK_COUNT_MAX);

            var readerChannelTask = Task.Run(async () =>
            {
                await foreach (var addressChange in addressChangesReaderCh.ReadAllAsync(cancellationTokenSource.Token).ConfigureAwait(false))
                {
                    if (addressChange.SequenceNumber > lastRunSequenceNumber)
                    {
                        addressChangesBulk.Add(addressChange);
                    }
                }
            }, cancellationTokenSource.Token);

            var bulkInsertTask = Task.Run(async () =>
            {
                try
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
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

                            _logger.LogInformation("Bulk inserting {BulkInsertCount}", changesCount);

                            _databaseAddressChangeIndex.BulkInsert(bulkInsertChanges);

                            _logger.LogInformation(
                                "Finished bulk inserting {Count} address changes.",
                                changesCount);
                        }

                        await Task.Delay(BULK_INSERT_DELAY_MS).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("{Exception}", ex);
                    await cancellationTokenSource.CancelAsync().ConfigureAwait(false);
                    throw;
                }
            }, cancellationTokenSource.Token);

            await Task.WhenAll(readerChannelTask, bulkInsertTask).ConfigureAwait(false);
        }, cancellationTokenSource.Token);

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
    }
}
