using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Common.Core.ResourceManagers;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace EM.Carts.Application.Validations;

[ExcludeFromCodeCoverage]
public sealed class UseCaseValidation<TRequest> : IUseCase<TRequest> where TRequest : IRequest
{
    private readonly IUseCase<TRequest> _useCase;
    private readonly IValidator<TRequest> _validator;
    private readonly IResourceManager _resourceManager;
    private IPresenter _presenter = default!;

    public UseCaseValidation(
        IUseCase<TRequest> useCase,
        IValidator<TRequest> validator,
        IResourceManager resourceManager)
    {
        _useCase = useCase;
        _validator = validator;
        _resourceManager = resourceManager;
    }

    public async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            await _useCase.ExecuteAsync(request, cancellationToken);
            return;
        }

        string[] keys = validationResult.Errors
            .Where(validateFailure => validateFailure is not null)
            .Select(failure => failure.ErrorMessage)
            .Distinct()
            .ToArray();

        Result result = await _resourceManager.GetErrorsByKeysAsync(keys, cancellationToken);
        _presenter.BadRequest(result.Errors);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _useCase.SetPresenter(presenter);
    }
}
