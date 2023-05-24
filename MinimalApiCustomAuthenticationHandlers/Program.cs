using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalApiCustomAuthenticationHandlers.Authorization;
using MinimalApiCustomAuthenticationHandlers.Authorization.Auth;
using MinimalApiCustomAuthenticationHandlers.Endpoints.Dummy;
using MinimalApiCustomAuthenticationHandlers.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(opt =>
{
    opt.UseInMemoryDatabase("DummyDb");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    o =>
    {
        o.User.RequireUniqueEmail = false;
        o.Password.RequireDigit = false;
        o.Password.RequiredLength = 3;
        o.Password.RequireLowercase = false;
        o.Password.RequireUppercase = false;
        o.Password.RequiredUniqueChars = 2;
        o.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(o =>
{
    // o.DefaultPolicy = new AuthorizationPolicyBuilder()
    //     .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
    //     .RequireClaim("role", "AcceptMyGreetings")
    //     .Build();

    o.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
    {
        policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
        policyBuilder.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
        // policyBuilder.RequireAuthenticatedUser();
        // policyBuilder.RequireClaim("role", "AcceptMyGreetings");
    });
});

var app = await builder.BuildAndSetup();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", 
        (ClaimsPrincipal user) => 
            user.Claims.Select(x => new {x.Type, x.Value})
    )
    .RequireNoPermission();

app.MapGet("/sign-in",
    async (SignInManager<IdentityUser> signInManager)
        =>
    {
        await signInManager.PasswordSignInAsync("Test@test.com", "test@123", false,false);
        return Results.Redirect("/");
    }).RequireNoPermission();


app.MapHell();

app.Run();