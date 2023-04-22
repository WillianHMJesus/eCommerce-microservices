using EM.Catalog.Application.Results;
using FluentValidation;
using MediatR;

namespace EM.Catalog.Application;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
     => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        List<Error> responseErrors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validateFailure => validateFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage
            ))
            .Distinct()
            .ToList();

        if (responseErrors.Any())
        {
            return (TResponse)Result.CreateResponseWithErrors(responseErrors);
        }

        return await next();
    }
}
