using Carter;
using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Application.Commands.RefreshUserToken;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;

namespace EM.Authentication.API.Oauth;

public sealed class OauthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/oauth")
            .WithOpenApi();

        group.MapPost("", AuthenticateAsync)
            .WithName("Authenticate");

        group.MapPost("refresh-token", RefreshTokenAsync)
            .WithName("RefreshTokenAsync")
            .RequireAuthorization();
    }

    private static async Task<IResult> AuthenticateAsync(
        [FromBody] CredentialsRequest request,
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

    private static async Task<IResult> RefreshTokenAsync(
        [FromServices] IMediator mediator,
        ClaimsPrincipal claims)
    {
        string nameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        var subClaim = claims.FindFirst(nameIdentifier)?.Value;

        if (!Guid.TryParse(subClaim, out _))
        {
            return Results.BadRequest(Result.CreateResponseWithErrors([new Error("ApplicationError", "The refresh token is invalid")]));
        }

        var command = new RefreshUserTokenCommand(Guid.Parse(subClaim));
        var result = await mediator.Send(command);

        return result.Success
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
