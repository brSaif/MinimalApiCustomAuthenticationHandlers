/*
 * Here lies the simpler approach to implement custom authorization handler by just defining
 * the logic you want the handler to execute.
 */


using Microsoft.AspNetCore.Authorization;
using MinimalApiCustomAuthenticationHandlers.Contracts;
using MinimalApiCustomAuthenticationHandlers.Endpoints.Dummy;

namespace MinimalApiCustomAuthenticationHandlers.Authorization.Auth;

/// <summary>
/// Represents the custom <see cref="HasPermissionAuthorizationRequirement"/> class.
/// </summary>
public class HasPermissionAuthorizationRequirement : AuthorizationHandler<HasPermissionAuthorizationRequirement>, IAuthorizationRequirement
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        HasPermissionAuthorizationRequirement requirement)
    {
        var permissions = context.User?.Claims
            .Select(x => new UserPermissionDto(x.Value))
            .ToList();

        var attribute = (context.Resource as DefaultHttpContext)
            .Request
            .HttpContext
            .GetEndpoint()
            .Metadata
            .GetMetadata<HasPermissionAttribute>();

        
        if (!await AuthorizeAsync(attribute.Name, permissions))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="permission">The required permission to access the endpoint.</param>
    /// <param name="permissions">The user permissions to check against.</param>
    /// <returns>True is authorization succeeded, false otherwise.</returns>
    private Task<bool> AuthorizeAsync(string permission, List<UserPermissionDto> permissions)
    {
        if (permission is null)
            return Task.FromResult(false);
        
#if !DEBUG
            return Task.FromResult(true);
#endif
        return Task.FromResult(permissions.Any(x => x.Code == permission));
    }
}