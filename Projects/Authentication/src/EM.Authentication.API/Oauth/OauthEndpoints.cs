using Carter;
using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Application.Commands.AuthenticateUser;
using Microsoft.AspNetCore.Mvc;
using WH.SharedKernel.Mediator;

namespace EM.Authentication.API.Oauth;

public sealed class OauthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api")
            .WithOpenApi();

        group.MapPost("oauth", OauthAsync)
            .WithName("Oauth");
    }

    private static async Task<IResult> OauthAsync(
        [FromBody] OauthRequest request,
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
