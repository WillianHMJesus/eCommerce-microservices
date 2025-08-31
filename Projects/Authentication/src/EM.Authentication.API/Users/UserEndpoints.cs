using Carter;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using Microsoft.AspNetCore.Mvc;
using WH.SharedKernel.Mediator;

namespace EM.Authentication.API.Users;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api")
            .WithOpenApi();

        group.MapPost("users/customer-profile", AddCustomerAsync)
            .WithName("AddCustomer");

        group.MapPost("users", AddUserAsync)
            .WithName("AddUser")
            .RequireAuthorization("AddUser");

        group.MapPost("oauth", AuthenticateUserAsync)
            .WithName("Oauth");
    }

    public static async Task<IResult> AddCustomerAsync(
        AddCustomerRequest request,
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

    public static async Task<IResult> AddUserAsync(
        AddUserRequest request,
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

    public static async Task<IResult> AuthenticateUserAsync(
        AuthenticateUserRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new AuthenticateUserCommand(
            request.EmailAddress,
            request.Password);

        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
