namespace EM.Common.Core.ResourceManagers;

public class Result
{
    public bool Success => !Errors.Any();
    public object? Data { get; set; }
    public IEnumerable<Error> Errors { get; set; } = new List<Error>();

    public static Result CreateResponseWithData(object data)
    {
        return new Result { Data = data };
    }

    public static Result CreateResponseWithErrors(IEnumerable<Error> errors)
    {
        return new Result { Errors = errors };
    }
}
