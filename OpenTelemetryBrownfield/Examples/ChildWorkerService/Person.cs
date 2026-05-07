using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChildWorkerService;

public class Person
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}