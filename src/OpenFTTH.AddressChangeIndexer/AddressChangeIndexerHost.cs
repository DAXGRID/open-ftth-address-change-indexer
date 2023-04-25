using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed class AddressChangeIndexerHost : BackgroundService
{
    private ILogger<AddressChangeIndexerHost> _logger;

    public AddressChangeIndexerHost(ILogger<AddressChangeIndexerHost> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {HostName}.", nameof(AddressChangeIndexerHost));
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
