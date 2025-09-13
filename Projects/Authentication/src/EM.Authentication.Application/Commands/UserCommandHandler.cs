using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Application.Mappers;
using EM.Authentication.Application.Providers;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using WH.SharedKernel;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;

namespace EM.Authentication.Application.Commands;

public sealed class UserCommandHandler(
    IUserRepository repository,
    IUserMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordProvider passwordProvider,
    ITokenProvider tokenProvider) :
    ICommandHandler<AddUserCommand>,
    ICommandHandler<AuthenticateUserCommand>,
    ICommandHandler<ChangeUserPasswordCommand>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetProfileByNameAsync(request.ProfileName, cancellationToken);

        if (profile is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", Profile.ProfileNotFound)]);
        }

        string passwordHash = passwordProvider.HashPassword(request.Password);
        User user = mapper.Map(request, passwordHash);
        user.AddProfile(profile!);

        await repository.AddAsync(user, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.ErrorSavingUser)]);
        }

        return Result.CreateResponseWithData(user.Id);
    }

    public async Task<Result> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByEmailAsync(request.EmailAddress, cancellationToken);

        if (!ValidateUserAuthenticity(user, request.Password))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.EmailAddressOrPasswordIncorrect)]);
        }

        var response = mapper.Map(user!);
        response.AccessToken = tokenProvider.Generate(user!);
        response.ExpirationToken = tokenProvider.GetTokenExpiration(response.AccessToken);
        response.RefreshToken = tokenProvider.GenerateRefreshToken(user!);

        return Result.CreateResponseWithData(response);
    }

    public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByEmailAsync(request.EmailAddress, cancellationToken);

        if (!ValidateUserAuthenticity(user, request.OldPassword))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.EmailAddressOrPasswordIncorrect)]);
        }

        string passwordHash = passwordProvider.HashPassword(request.NewPassword);
        user!.ChangePasswordHash(passwordHash);
        
        repository.Update(user);

        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.ErrorSavingUser)]);
        }

        return Result.CreateResponseWithData(user.Id);
    }

    private bool ValidateUserAuthenticity(User? user, string password)
    {
        if (user is null) return false;

        return passwordProvider.VerifyHashedPassword(user.PasswordHash, password);
    }
}
