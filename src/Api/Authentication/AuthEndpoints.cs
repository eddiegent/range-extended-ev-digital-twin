using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;

namespace RangeExtendedEvDigitalTwin.Api.Authentication;

public static class AuthEndpoints
{
    private static readonly string MissingUserPasswordHash = new PasswordHasher<OperatorUser>()
        .HashPassword(new OperatorUser(), "Missing-user-password-1!");

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapGet(
                "/status",
                async Task<IResult> (
                    ClaimsPrincipal principal,
                    UserManager<OperatorUser> userManager) =>
                {
                    if (principal.Identity?.IsAuthenticated != true)
                    {
                        return Results.Ok(AuthStatusResponse.SignedOut);
                    }

                    var user = await userManager.GetUserAsync(principal);

                    return Results.Ok(AuthStatusResponse.FromUser(user));
                })
            .AllowAnonymous();

        group.MapPost(
                "/sign-in",
                async Task<IResult> (
                    SignInRequest request,
                    UserManager<OperatorUser> userManager,
                    SignInManager<OperatorUser> signInManager) =>
                {
                    var user = await userManager.FindByEmailAsync(request.Email);

                    if (user is null)
                    {
                        _ = userManager.PasswordHasher.VerifyHashedPassword(
                            new OperatorUser(),
                            MissingUserPasswordHash,
                            request.Password);
                        return Results.Unauthorized();
                    }

                    var signInResult = await signInManager.PasswordSignInAsync(
                        user,
                        request.Password,
                        isPersistent: true,
                        lockoutOnFailure: true);

                    if (!signInResult.Succeeded)
                    {
                        return Results.Unauthorized();
                    }

                    return Results.Ok(AuthStatusResponse.FromUser(user));
                })
            .AllowAnonymous();

        group.MapPost(
                "/sign-out",
                async Task<IResult> (SignInManager<OperatorUser> signInManager) =>
                {
                    await signInManager.SignOutAsync();
                    return Results.NoContent();
                })
            .AllowAnonymous();

        return endpoints;
    }

    public sealed record SignInRequest(string Email, string Password);

    public sealed record AuthStatusResponse(bool IsAuthenticated, string? Email, string? UserName)
    {
        public static AuthStatusResponse SignedOut { get; } = new(false, null, null);

        public static AuthStatusResponse FromUser(OperatorUser? user) =>
            user is null
                ? SignedOut
                : new(true, user.Email, user.UserName);
    }
}
