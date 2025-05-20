using System;
using System.Threading.Tasks;
using ChildWorkerService.Messages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NServiceBus;
using WebApplication.Messages;

namespace ChildWorkerService;

public class YellItHandler : IHandleMessages<SomethingSaid>
{
    private readonly ILogger<YellItHandler> _logger;
    private readonly IMongoClient _client;

    private static readonly Random _coinFlip = new Random();

    public YellItHandler(ILogger<YellItHandler> logger, IMongoClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task Handle(SomethingSaid message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Yelling out {message}", message.Message);

        //if (_coinFlip.Next(2) == 0)
        //{
        //    throw new Exception("Something went wrong!");
        //}

        var database = _client.GetDatabase("dev");
        var collection = database.GetCollection<Person>(nameof(Person));

        var count = await collection.CountDocumentsAsync(p => true, cancellationToken: context.CancellationToken);
        var rng = new Random();

        var favoritePerson = await collection.AsQueryable().Skip(rng.Next((int)count)).FirstAsync(context.CancellationToken);

        await context.Publish(new SomethingYelled
        {
            Message = message.Message.ToUpperInvariant(),
            Id = message.Id,
            FavoritePerson = $"{favoritePerson.FirstName} {favoritePerson.LastName}"
        });
    }

}