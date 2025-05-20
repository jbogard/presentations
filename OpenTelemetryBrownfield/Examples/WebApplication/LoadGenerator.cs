using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using WorkerService.Messages;

namespace WebApplication;

public class LoadGenerator : BackgroundService
{
    private readonly IMessageSession _messageSession;

    public LoadGenerator(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(50, stoppingToken);

            var message = new SaySomething
            {
                Id = Guid.NewGuid(),
                Message = "Saying hello from load test"
            };

            await _messageSession.Send(message);
        }
    }
}