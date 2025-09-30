using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Application.Commands.RefreshUserToken;
using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.Application.Commands.SendUserToken;
using EM.Authentication.Application.Commands.ValidateUserToken;
using EM.Authentication.Application.Providers;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.Notifications;
using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.ResourceManagers;
using WH.SimpleMapper;

namespace EM.Authentication.Application.Commands;

public sealed class UserCommandHandler(
    IUserRepository repository,
    IUnitOfWork unitOfWork,
    IPasswordProvider passwordProvider,
    ITokenProvider tokenProvider,
    IUserEmailNotification emailService,
    IMapper mapper) :
    ICommandHandler<AddUserCommand>,
    ICommandHandler<AuthenticateUserCommand>,
    ICommandHandler<ChangeUserPasswordCommand>,
    ICommandHandler<SendUserTokenCommand>,
    ICommandHandler<ValidateUserTokenCommand>,
    ICommandHandler<ResetUserPasswordCommand>,
    ICommandHandler<RefreshUserTokenCommand>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetProfileByNameAsync(request.ProfileName, cancellationToken);

        if (profile is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", Profile.ProfileNotFound)]);
        }

        string passwordHash = passwordProvider.HashPassword(request.Password);
        User user = mapper.Map<(AddUserCommand, string), User>((request, passwordHash));
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

        var response = mapper.Map<User, UserResponse>(user!);
        response.AccessToken = tokenProvider.GenerateJwtToken(user!);
        response.TokenExpiration = tokenProvider.GetJwtTokenExpiration(response.AccessToken);
        response.RefreshToken = tokenProvider.GenerateJwtRefreshToken(user!);

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

    public async Task<Result> Handle(SendUserTokenCommand request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByEmailAsync(request.EmailAddress, cancellationToken);

        if (user is null)
        {
            return Result.CreateResponseWithData(Guid.NewGuid());
        }

        var token = tokenProvider.GenerateSecurityToken();
        var tokenHash = passwordProvider.HashPassword(token);
        var userToken = mapper.Map<(Guid UserId, string TokenHash, short MinutesExpire), UserToken>(
            (user.Id, tokenHash, UserToken.SecurityTokenExpirationTimeInMinutes));

        await repository.AddTokenAsync(userToken, cancellationToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.ErrorSavingUserToken)]);
        }

        emailService.SendPasswordResetEmail(request.EmailAddress, token);
        return Result.CreateResponseWithData(userToken.Id);
    }

    public async Task<Result> Handle(ValidateUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = await repository.GetTokenByIdAsync(request.UserTokenId, cancellationToken);

        if (userToken is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.UserTokenNotFound)]);
        }

        if (userToken.ExpiresAt < DateTime.Now)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.UserTokenExpired)]);
        }

        if (!passwordProvider.VerifyHashedPassword(userToken.TokenHash, request.Token))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.InvalidToken)]);
        }

        userToken.SetValidation();
        repository.UpdateToken(userToken);

        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.ErrorSavingUserToken)]);
        }

        return Result.CreateResponseWithData();
    }

    public async Task<Result> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var userToken = await repository.GetTokenByIdAsync(request.UserTokenId, cancellationToken);

        if (userToken is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.UserTokenNotFound)]);
        }

        if (!userToken.Validated)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", UserToken.UserTokenNotValidated)]);
        }

        var newPasswordHash = passwordProvider.HashPassword(request.NewPassword);
        userToken.User.ChangePasswordHash(newPasswordHash);
        repository.Update(userToken.User);

        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.ErrorSavingUser)]);
        }

        return Result.CreateResponseWithData();
    }

    public async Task<Result> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", User.UserNotFound)]);
        }

        var response = mapper.Map<User, UserResponse>(user!);
        response.AccessToken = tokenProvider.GenerateJwtToken(user!);
        response.TokenExpiration = tokenProvider.GetJwtTokenExpiration(response.AccessToken);
        response.RefreshToken = tokenProvider.GenerateJwtRefreshToken(user!);

        return Result.CreateResponseWithData(response);
    }

    private bool ValidateUserAuthenticity(User? user, string password)
    {
        if (user is null) return false;

        return passwordProvider.VerifyHashedPassword(user.PasswordHash, password);
    }
}
