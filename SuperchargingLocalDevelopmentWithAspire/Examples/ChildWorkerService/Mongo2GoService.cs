using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace ChildWorkerService;

public class Mongo2GoService : IHostedService
{
    private readonly IMongoClient _client;

    public Mongo2GoService(IMongoClient client)
    {
        _client = client;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase("dev");
        var collection = database.GetCollection<Person>(nameof(Person));

        return collection.InsertManyAsync(
        [
            new Person
            {
                FirstName = "Homer",
                LastName = "Simpson"
            }, 
            new Person
            {
                FirstName = "Marge",
                LastName = "Simpson"
            }, 
            new Person
            {
                FirstName = "Bart",
                LastName = "Simpson"
            }, 
            new Person
            {
                FirstName = "Lisa",
                LastName = "Simpson"
            }, 
            new Person
            {
                FirstName = "Maggie",
                LastName = "Simpson"
            }, 
        ], cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}