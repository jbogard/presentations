using NServiceBus;

namespace ChildWorkerService.Messages;

public class MakeItYellResponse : IMessage
{
    public string Value { get; set; }
    public string FavoritePerson { get; set; }
}