using NServiceBus;

namespace ChildWorkerService.Messages;

public class MakeItYell : ICommand
{
    public string Value { get; set; }
}