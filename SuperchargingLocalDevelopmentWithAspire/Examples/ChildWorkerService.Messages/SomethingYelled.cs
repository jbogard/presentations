using System;
using NServiceBus;

namespace ChildWorkerService.Messages;

public class SomethingYelled : IEvent
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string FavoritePerson { get; set; }
}