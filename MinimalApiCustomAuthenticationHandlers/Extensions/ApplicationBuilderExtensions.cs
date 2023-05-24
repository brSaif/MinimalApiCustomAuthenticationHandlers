using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MinimalApiCustomAuthenticationHandlers.Endpoints.Dummy;

namespace MinimalApiCustomAuthenticationHandlers.Extensions;

/// <summary>
/// Represents the <see cref="ApplicationBuilderExtensions"/> class.
/// </summary>
public static class ApplicationBuilderExtensions
{
    
    /// <summary>
    /// Builds the <see cref="WebApplication"/> and spin up a demo <see cref="IdentityUser"/>. 
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application.</returns>
    public static async Task<WebApplication> BuildAndSetup(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var user = new IdentityUser() {UserName = "Test@test.com", Email = "Test@test.com"};
        await userManager.CreateAsync(user, "test@123");
        await userManager.AddClaimsAsync(
            user, 
            new []
            {
                new Claim("role", DummyPermissions.AcceptMyGreetings),
                new Claim("role", DummyPermissions.GreetMeBack),
                new Claim("role", DummyPermissions.MakeSillyJokes)
            });
        
        return app;
    }
}