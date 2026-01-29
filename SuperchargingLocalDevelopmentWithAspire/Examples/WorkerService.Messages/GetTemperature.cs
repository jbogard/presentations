using NServiceBus;

namespace WorkerService.Messages;

public class GetTemperature : ICommand
{
        
}

public class GetTemperatureResponse : IMessage
{
    public int Value { get; set; }
}