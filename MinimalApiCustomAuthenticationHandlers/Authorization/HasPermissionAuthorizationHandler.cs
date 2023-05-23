using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Represents the custom <see cref="HasPermissionAuthorizationHandler"/> class.
/// </summary>
internal class HasPermissionAuthorizationHandler : AttributeAuthorizationHandler<
    HasPermissionAuthorizationRequirement, HasPermissionAttribute>
{
    

    /// <summary>
    /// Initializes a new instance of <see cref="HasPermissionAuthorizationHandler"/>
    /// </summary>
    /// <remarks> Normally you would inject your dependencies via the constructor</remarks>
    public HasPermissionAuthorizationHandler()
    {
       
    }

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasPermissionAuthorizationRequirement requirement,
        HasPermissionAttribute attribute)
    {
        var permissions = new List<UserPermissionDto>();

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
#if !DEBUG
            return Task.FromResult(true);
#endif
        return Task.FromResult(permissions.Any(x => x.Code == permission));
    }
    
    internal class UserPermissionDto
    {
        public string Code { get; set; }
    }
}