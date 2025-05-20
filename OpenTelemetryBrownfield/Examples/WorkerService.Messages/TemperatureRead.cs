using System;
using NServiceBus;

namespace WorkerService.Messages;

public class TemperatureRead : IEvent
{
    public int Value { get; set; }
    public Guid Id { get; set; }
}