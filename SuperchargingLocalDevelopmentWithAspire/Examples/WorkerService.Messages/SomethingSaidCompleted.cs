using NServiceBus;

namespace WorkerService.Messages;

public class SomethingSaidCompleted : IEvent
{
    public string Message { get; set; }
}