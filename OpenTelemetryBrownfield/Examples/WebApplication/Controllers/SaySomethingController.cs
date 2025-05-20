using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using WebApplication.Messages;
using WorkerService.Messages;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class SaySomethingController : ControllerBase
{
    private readonly ILogger<SaySomethingController> _logger;
    private readonly IMessageSession _messageSession;

    public SaySomethingController(ILogger<SaySomethingController> logger, 
        IMessageSession messageSession)
    {
        _logger = logger;
        _messageSession = messageSession;
    }

    [HttpGet]
    public async Task<ActionResult<Guid>> Get(string message)
    {
        var command = new SaySomething
        {
            Message = message,
            Id = Guid.NewGuid()
        };

        Activity.Current?.AddBaggage("operation.id", command.Id.ToString());

        await _messageSession.Send(command);

        return Accepted(command.Id);
    }

    [HttpGet("else")]
    public async Task<ActionResult<Guid>> Else(string message)
    {
        var @event = new SomethingSaid
        {
            Message = message,
            Id = Guid.NewGuid()
        };

        _logger.LogInformation("Publishing message {message} with {id}", @event.Message, @event.Id);

        Activity.Current?.AddBaggage("operation.id", @event.Id.ToString());

        await _messageSession.Publish(@event);

        return Accepted(@event.Id);
    }

}