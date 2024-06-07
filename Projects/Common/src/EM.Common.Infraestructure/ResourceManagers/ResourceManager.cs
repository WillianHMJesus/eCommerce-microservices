using Em.Common.Infraestructure.DatabaseSettings;
using EM.Common.Core.ResourceManagers;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Em.Common.Infraestructure.ResourceManagers;

public sealed class ResourceManager : IResourceManager
{
    private readonly IMongoCollection<Error> _errorsCollection;

    public ResourceManager(IOptions<MessageDatabaseSettings> options)
    {
        InitDbSettings();

        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(options.Value.DatabaseName);

        _errorsCollection = database.GetCollection<Error>(options.Value.ErrorsCollectionName);
    }

    private void InitDbSettings()
    {
        var ignoreExtraElements = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", ignoreExtraElements, type => true);

        var camelCaseElement = new ConventionPack() { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElement", camelCaseElement, type => true);
    }

    public async Task<Result> GetErrorsByKeyAsync(string key, CancellationToken cancellationToken)
    {
        IEnumerable<Error> errors = await _errorsCollection.Find(x => x.Key == key).ToListAsync(cancellationToken);

        if (errors.Count() == 0)
        {
            errors = GetDefaultValue();
        }

        return Result.CreateResponseWithErrors(errors);
    }

    public async Task<Result> GetErrorsByKeysAsync(string[] keys, CancellationToken cancellationToken)
    {
        FilterDefinitionBuilder<Error> builder = Builders<Error>.Filter;
        FilterDefinition<Error> filter = builder.In(x => x.Key, keys);

        IEnumerable<Error> errors = await _errorsCollection.Find(filter).ToListAsync(cancellationToken);

        if (errors.Count() == 0)
        {
            errors = GetDefaultValue();
        }

        return Result.CreateResponseWithErrors(errors);
    }

    private IEnumerable<Error> GetDefaultValue()
    {
        return new List<Error> 
        { 
            new Error("ErrorMessageNotRegistered", "Ocorreu um erro, porém a mensagem não foi registrada.") 
        };
    }
}
