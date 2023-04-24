namespace EM.Catalog.Application.Results;

public class Result
{
    public bool Success => !Errors.Any();
    public object? Data { get; set; }
    public IList<Error> Errors { get; set; } = new List<Error>();

    public static Result CreateResponseWithData(object data)
    {
        return new Result { Data = data };
    }

    public static Result CreateResponseWithErrors(List<Error> errors)
    {
        return new Result { Errors = errors };
    }

    public static Result CreateResponseWithErrors(string key, string message)
    {
        Error error = new(key, message);

        return new Result
        {
            Errors = new List<Error> { error }
        };
    }
}
