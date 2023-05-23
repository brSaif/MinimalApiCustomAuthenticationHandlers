using Microsoft.AspNetCore.Authorization;

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
        var attribute = (context.Resource as RouteEndpoint)?.Metadata.GetMetadata<TAttribute>();

        return HandleRequirementAsync(context, requirement, attribute);
    }
    
    protected abstract Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TAttribute attribute);
}