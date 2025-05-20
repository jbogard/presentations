using System;
using NServiceBus;

namespace WebApplication.Messages;

public class SomethingSaid : IEvent
{
    public string Message { get; set; }
    public Guid Id { get; set; }
}