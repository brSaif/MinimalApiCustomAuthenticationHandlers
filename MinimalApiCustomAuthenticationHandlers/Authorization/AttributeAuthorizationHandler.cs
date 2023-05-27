/*
 * Here lies a more customizable AttributeAuthorizationHandler, that is gonna be registered as a Scoped
 * implementation of 'IAuthorizationHandler' so wa can make good use of IoC registered services.
 *
 * This solution is sightly tweaked version from the original one described here 
 * 'https://github.com/kgrzybek/modular-monolith-with-ddd/blob/master/src/API/CompanyName.MyMeetings.API/Configuration/Authorization/AttributeAuthorizationHandler.cs'
 * 
 */


using Microsoft.AspNetCore.Authorization;

namespace MinimalApiCustomAuthenticationHandlers.Authorization;

/// <summary>
/// Represents the custom <see cref="AttributeAuthorizationHandler{TRequirement,TAttribute}"/> class.
/// </summary>
/// <typeparam name="TRequirement">The authorization requirement.</typeparam>
/// <typeparam name="TAttribute">The authorization attribute.</typeparam>
public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute>
    : AuthorizationHandler<TRequirement>
    where TRequirement : IAuthorizationRequirement
    where TAttribute : Attribute
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
    {
        var attribute =  (context.Resource as DefaultHttpContext)
            .Request
            .HttpContext
            .GetEndpoint()
            .Metadata
            .GetMetadata<TAttribute>();
        
        return HandleRequirementAsync(context, requirement, attribute);
    }
    
    protected abstract Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TAttribute attribute);
}