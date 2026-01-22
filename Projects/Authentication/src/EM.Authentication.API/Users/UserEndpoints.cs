using Azure.Core;
using Carter;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.Application.Commands.SendUserToken;
using EM.Authentication.Application.Commands.ValidateUserToken;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WH.SharedKernel.Mediator;

namespace EM.Authentication.API.Users;

public sealed class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users")
            .WithOpenApi();

        group.MapPost("customer-profile", AddCustomerAsync)
            .WithName("AddCustomer");

        group.MapPost("", AddUserAsync)
            .WithName("AddUser")
            .RequireAuthorization("AddUser");

        group.MapPut("change-password", ChangeUserPasswordAsync)
            .WithName("ChangeUserPassword")
            .RequireAuthorization();

        group.MapPost("send-token", SendUserTokenAsync)
            .WithName("SendUserToken")
            .RequireAuthorization();

        group.MapPost("validate-token", ValidateUserTokenAsync)
            .WithName("ValidateUserToken")
            .RequireAuthorization();

        group.MapPost("reset-password", ResetUserPasswordAsync)
            .WithName("ResetUserPassword")
            .RequireAuthorization();
    }

    private static async Task<IResult> AddCustomerAsync(
        [FromBody] AddCustomerRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new AddUserCommand(
            request.UserName,
            request.EmailAddress,
            request.Password,
            request.ConfirmPassword,
            "Customer");

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> AddUserAsync(
        [FromBody] AddUserRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new AddUserCommand(
            request.UserName,
            request.EmailAddress,
            request.Password,
            request.ConfirmPassword,
            request.ProfileName);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> ChangeUserPasswordAsync(
        [FromBody] ChangeUserPasswordRequest request,
        [FromServices] IMediator mediator,
        ClaimsPrincipal user)
    {
        if (!ValidateProfileAndEmail(user, request.EmailAddress))
        {
            return Results.Forbid();
        }

        var command = new ChangeUserPasswordCommand(
            request.EmailAddress,
            request.OldPassword,
            request.NewPassword,
            request.ConfirmPassword);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> SendUserTokenAsync(
        [FromBody] SendUserTokenRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new SendUserTokenCommand(request.EmailAddress);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> ValidateUserTokenAsync(
        [FromBody] ValidateUserTokenRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new ValidateUserTokenCommand(
            request.UserTokenId,
            request.Token);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> ResetUserPasswordAsync(
        [FromBody] ResetPasswordRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new ResetUserPasswordCommand(
            request.UserTokenId,
            request.NewPassword,
            request.ConfirmPassword);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }

    private static bool ValidateProfileAndEmail(ClaimsPrincipal user, string emailAddress)
    {
        var emailAddressLogged = user.FindFirst(ClaimTypes.Email)?.Value;

        return user.HasClaim(ClaimTypes.Role, "ChangeUserPassword") ||
            emailAddressLogged == emailAddress;
    }
}
