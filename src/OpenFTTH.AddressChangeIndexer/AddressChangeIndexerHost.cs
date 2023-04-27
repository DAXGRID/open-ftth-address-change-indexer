using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenFTTH.EventSourcing;
using System.Diagnostics;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed class AddressChangeIndexerHost : BackgroundService
{
    //const int CATCHUP_TIME_MS = 60_000;
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

        _logger.LogInformation("Starting dehydration.");
        await _eventStore
            .DehydrateProjectionsAsync(stoppingToken)
            .ConfigureAwait(false);

        _logger.LogInformation(
            "Memory after dehydration {MibiBytes}.",
            Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024);

        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await Task.Delay(CATCHUP_TIME_MS, stoppingToken).ConfigureAwait(false);
        //     _logger.LogDebug("Checking for new events.");
        //     var changes = await _eventStore
        //         .CatchUpAsync(stoppingToken)
        //         .ConfigureAwait(false);
        // }
    }
}
