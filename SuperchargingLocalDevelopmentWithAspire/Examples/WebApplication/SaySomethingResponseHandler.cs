using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using WorkerService.Messages;

namespace WebApplication;

public class SaySomethingResponseHandler : IHandleMessages<SaySomethingResponse>
{
    private readonly ILogger<SaySomethingResponseHandler> _logger;

    public SaySomethingResponseHandler(ILogger<SaySomethingResponseHandler> logger)
        => _logger = logger;

    public Task Handle(SaySomethingResponse message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Received {message}", message.Message);

        return Task.CompletedTask;
    }
}