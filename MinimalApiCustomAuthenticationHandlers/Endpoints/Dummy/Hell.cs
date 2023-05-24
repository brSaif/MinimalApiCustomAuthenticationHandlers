using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MinimalApiCustomAuthenticationHandlers.Authorization;
using MinimalApiCustomAuthenticationHandlers.Extensions;

namespace MinimalApiCustomAuthenticationHandlers.Endpoints.Dummy;

/// <summary>
/// Represents a dummy minimal-api routing group.
/// </summary>
public static class Hell
{
    /// <summary>
    /// Maps The 'Dummy' routing group and its subsequent endpoints.
    /// </summary>
    /// <param name="builder">The route builder.</param>
    /// <returns>Route group builder.</returns>
    public static RouteGroupBuilder MapHell(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup("/hell")
            .RequirePermission();

        group.MapGet("/dente",
            () => 
                $"You need the '{nameof(DummyPermissions.AcceptMyGreetings)}' permission to get to dente's hell :p!.")
            .RequirePermission(DummyPermissions.AcceptMyGreetings);
        
        group.MapGet("/Abu-al-alaa", 
                () => "Abu al alaa is much more generous, no permission required to get to his hell lol!.")
            .RequireNoPermission();
        
        group.MapGet("/gb", 
                () => "Well if you accepted my greetings, i expect you greet me back. " +
                      $"That's if you have the {nameof(DummyPermissions.GreetMeBack)} permission.")
            .RequirePermission(DummyPermissions.GreetMeBack);
        
        group.MapGet("/make-silly-jokes", 
                (string request) => $"{request}")
            .RequirePermission(DummyPermissions.MakeSillyJokes);

        return group;
    }
}