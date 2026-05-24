using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RangeExtendedEvDigitalTwin.Infrastructure.Authentication;

namespace RangeExtendedEvDigitalTwin.Api.Tests;

public sealed class AuthenticatedOperatorFlowTests
{
    [Fact]
    public async Task Bootstrapped_operator_can_sign_in_read_auth_status_and_sign_out()
    {
        await using var host = await ApiTestHost.StartAsync();
        using var client = host.Client;

        var initialStatus = await client.GetFromJsonAsync<AuthStatusResponse>("/api/auth/status");

        Assert.NotNull(initialStatus);
        Assert.False(initialStatus.IsAuthenticated);

        var signInResponse = await client.PostAsJsonAsync(
            "/api/auth/sign-in",
            new SignInRequest("operator@local.test", "Passw0rd"));

        Assert.Equal(HttpStatusCode.OK, signInResponse.StatusCode);

        var authenticatedStatus = await signInResponse.Content.ReadFromJsonAsync<AuthStatusResponse>();

        Assert.NotNull(authenticatedStatus);
        Assert.True(authenticatedStatus.IsAuthenticated);
        Assert.Equal("operator@local.test", authenticatedStatus.Email);
        ApplyAuthCookie(client, signInResponse);

        var currentStatus = await client.GetFromJsonAsync<AuthStatusResponse>("/api/auth/status");

        Assert.NotNull(currentStatus);
        Assert.True(currentStatus.IsAuthenticated);
        Assert.Equal("operator@local.test", currentStatus.Email);

        var signOutResponse = await client.PostAsync("/api/auth/sign-out", content: null);

        Assert.Equal(HttpStatusCode.NoContent, signOutResponse.StatusCode);
        client.DefaultRequestHeaders.Remove("Cookie");

        var signedOutStatus = await client.GetFromJsonAsync<AuthStatusResponse>("/api/auth/status");

        Assert.NotNull(signedOutStatus);
        Assert.False(signedOutStatus.IsAuthenticated);
    }

    [Fact]
    public async Task Repeated_failed_sign_in_attempts_lock_the_bootstrapped_operator()
    {
        await using var host = await ApiTestHost.StartAsync();
        using var client = host.Client;

        for (var attempt = 0; attempt < 5; attempt++)
        {
            var failedResponse = await client.PostAsJsonAsync(
                "/api/auth/sign-in",
                new SignInRequest("operator@local.test", "WrongPass1"));

            Assert.Equal(HttpStatusCode.Unauthorized, failedResponse.StatusCode);
        }

        var lockedOutResponse = await client.PostAsJsonAsync(
            "/api/auth/sign-in",
            new SignInRequest("operator@local.test", "Passw0rd"));

        Assert.Equal(HttpStatusCode.Unauthorized, lockedOutResponse.StatusCode);

        using var scope = host.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OperatorUser>>();
        var user = await userManager.FindByEmailAsync("operator@local.test");

        Assert.NotNull(user);
        Assert.True(await userManager.IsLockedOutAsync(user));
    }

    public sealed record AuthStatusResponse(bool IsAuthenticated, string? Email, string? UserName);

    public sealed record SignInRequest(string Email, string Password);

    private static void ApplyAuthCookie(HttpClient client, HttpResponseMessage signInResponse)
    {
        if (!signInResponse.Headers.TryGetValues("Set-Cookie", out var values))
        {
            return;
        }

        var cookieHeader = string.Join(
            "; ",
            values.Select(value => value.Split(';', 2, StringSplitOptions.TrimEntries)[0]));

        client.DefaultRequestHeaders.Remove("Cookie");
        client.DefaultRequestHeaders.Add("Cookie", cookieHeader);
    }
}
