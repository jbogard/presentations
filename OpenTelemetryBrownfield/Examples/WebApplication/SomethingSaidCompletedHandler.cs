using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using WorkerService.Messages;

namespace WebApplication;

public class SomethingSaidCompletedHandler : IHandleMessages<SomethingSaidCompleted>
{
    private readonly ILogger<SomethingSaidCompletedHandler> _logger;

    public SomethingSaidCompletedHandler(ILogger<SomethingSaidCompletedHandler> logger)
        => _logger = logger;

    public Task Handle(SomethingSaidCompleted message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Received {message}", message.Message);

        return Task.CompletedTask;
    }
}