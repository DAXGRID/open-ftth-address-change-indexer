﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenFTTH.EventSourcing;

namespace OpenFTTH.AddressChangeIndexer;

internal static class Program
{
    public static async Task Main()
    {
        using var host = HostConfig.Configure();
        var logger = host.Services
            .GetService<ILoggerFactory>()
            !.CreateLogger(nameof(Program));
        var eventStore = host.Services.GetService<IEventStore>();

        try
        {
            if (logger is null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ILogger)} is not configured.");
            }

            host.Services.GetService<IEventStore>()!.ScanForProjections();

            await host.RunAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Environment.ExitCode = 1;
            logger!.LogCritical("{Exception}", ex);
            throw;
        }
        finally
        {
            logger.LogInformation("Shutting down...");
        }
    }
}
