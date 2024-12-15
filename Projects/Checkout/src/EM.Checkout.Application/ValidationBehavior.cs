using EM.Common.Core.ResourceManagers;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EM.Checkout.Application;

[ExcludeFromCodeCoverage]
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IResourceManager _resourceManager;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        IResourceManager resourceManager)
    {
        _validators = validators;
        _resourceManager = resourceManager;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        ValidationResult[] validationResult = await Task.WhenAll(_validators.Select(async x => await x.ValidateAsync(request, cancellationToken)));

        string[] keys = validationResult
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validateFailure => validateFailure is not null)
            .Select(failure => failure.ErrorMessage)
            .Distinct()
            .ToArray();

        if (keys.Any())
        {
            return (TResponse)await _resourceManager.GetErrorsByKeysAsync(keys, cancellationToken);
        }

        return await next();
    }
}
