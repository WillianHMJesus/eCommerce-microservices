using EM.Catalog.Application.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EM.Catalog.Application;

[ExcludeFromCodeCoverage]
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

        ValidationResult[] validationResult = await Task.WhenAll(_validators.Select(async x => await x.ValidateAsync(request, cancellationToken)));
        
        List<Error> errors = validationResult
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validateFailure => validateFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToList();

        if (errors.Any())
        {
            return (TResponse)Result.CreateResponseWithErrors(errors);
        }

        return await next();
    }
}
