using EM.Common.Core.ResourceManagers;
using EM.Common.Infraestructure;
using MongoDB.Driver;

namespace Em.Common.Infraestructure.ResourceManagers;

public sealed class ResourceManager : IResourceManager
{
    private readonly MessageContext _context;

    public ResourceManager(MessageContext context)
    {
        _context = context;
    }

    public async Task<Result> GetErrorsByKeyAsync(string key, CancellationToken cancellationToken)
    {
        IEnumerable<Error> errors = await _context.Errors.Find(x => x.Key == key).ToListAsync(cancellationToken);

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

        IEnumerable<Error> errors = await _context.Errors.Find(filter).ToListAsync(cancellationToken);

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
