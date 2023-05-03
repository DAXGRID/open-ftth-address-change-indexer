using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenFTTH.EventSourcing;
using System.Diagnostics;
using System.Text.Json;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed class AddressChangeIndexerHost : BackgroundService
{
    const int CATCHUP_TIME_MS = 60_000;
    private ILogger<AddressChangeIndexerHost> _logger;
    private IEventStore _eventStore;

    public AddressChangeIndexerHost(
        ILogger<AddressChangeIndexerHost> logger,
        IEventStore eventStore)
    {
        _logger = logger;
        _eventStore = eventStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {HostName}.", nameof(AddressChangeIndexerHost));

        var addressChangeProjection = _eventStore.Projections.Get<AddressChangeProjection>();
        var addressChangesReaderCh = addressChangeProjection.AddressChanges;

        var dehydrateTask = Task.Run(async () =>
        {
            _logger.LogInformation("Starting dehydration.");
            await _eventStore
                .DehydrateProjectionsAsync(stoppingToken)
                .ConfigureAwait(false);

            _logger.LogInformation(
                "Memory after dehydration {MibiBytes}.",
                Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(CATCHUP_TIME_MS, stoppingToken).ConfigureAwait(false);
                _logger.LogDebug("Checking for new events.");
                var changes = await _eventStore
                    .CatchUpAsync(stoppingToken)
                    .ConfigureAwait(false);
            }
        }, stoppingToken);

        var processAddressChangesTask = Task.Run(async () =>
        {
            _logger.LogInformation("Starting address change processor.");
            var addressChangesReader = addressChangesReaderCh.ReadAllAsync(stoppingToken).ConfigureAwait(false);
            await foreach (var addressChange in addressChangesReader)
            {
                _logger.LogInformation("{AddressChange}", JsonSerializer.Serialize(addressChange));
            }
        }, stoppingToken);

        try
        {
            await Task.WhenAll(dehydrateTask, processAddressChangesTask).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Do nothing we do not care when operation is canceled, it will always be because of a shutdown request from OS.
            _logger.LogInformation("OperationCanceledException, requested shutdown...");
        }
    }
}
