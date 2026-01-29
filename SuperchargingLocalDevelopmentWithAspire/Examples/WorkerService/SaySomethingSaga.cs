using System;
using System.Threading.Tasks;
using ChildWorkerService.Messages;
using NServiceBus;
using WorkerService.Messages;

namespace WorkerService;

public class SaySomethingSagaData : ContainSagaData
{
    public string Message { get; set; }
    public Guid OriginatorId { get; set; }
    public int Temp { get; set; }
}

public class SaySomethingSaga : Saga<SaySomethingSagaData>,
    IAmStartedByMessages<SaySomething>,
    IHandleMessages<GetTemperatureResponse>,
    IHandleMessages<MakeItYellResponse>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SaySomethingSagaData> mapper)
    {
        mapper.MapSaga(s => s.OriginatorId).ToMessage<SaySomething>(m => m.Id);
    }

    public Task Handle(SaySomething message, IMessageHandlerContext context)
    {
        Data.Message = message.Message;

        return context.SendLocal(new GetTemperature());
    }

    public Task Handle(GetTemperatureResponse message, IMessageHandlerContext context)
    {
        Data.Temp = message.Value;

        var value = $"Back at ya {Data.Message} and it's {Data.Temp}F outside.";

        return context.Send(new MakeItYell {Value = value});
    }

    public Task Handle(MakeItYellResponse message, IMessageHandlerContext context)
    {
        MarkAsComplete();

        return ReplyToOriginator(context, new SaySomethingResponse
        {
            Message = message.Value + " and the favorite person is " + message.FavoritePerson
        });
    }
}