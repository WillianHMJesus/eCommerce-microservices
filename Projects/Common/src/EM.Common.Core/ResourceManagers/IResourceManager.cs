namespace EM.Common.Core.ResourceManagers;

public interface IResourceManager
{
    Task<Result> GetErrorsByKeyAsync(string key, CancellationToken cancellationToken);
    Task<Result> GetErrorsByKeysAsync(string[] keys, CancellationToken cancellationToken);
}
