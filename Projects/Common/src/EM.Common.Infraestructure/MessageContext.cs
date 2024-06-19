using EM.Common.Core.ResourceManagers;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace EM.Common.Infraestructure;
public sealed class MessageContext
{
    private readonly IMongoDatabase _database;

    public MessageContext(IMongoClient client)
    {
        _database = client.GetDatabase("Message");

        var ignoreExtraElements = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", ignoreExtraElements, type => true);

        var camelCaseElement = new ConventionPack() { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElement", camelCaseElement, type => true);
    }

    public IMongoCollection<Error> Errors
        => _database.GetCollection<Error>("Errors");
}
