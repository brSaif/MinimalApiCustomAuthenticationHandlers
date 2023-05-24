// using System.Security.Claims;
// using Microsoft.AspNetCore.Authorization;
// using MinimalApiCustomAuthenticationHandlers.Authorization.Auth;
// using MinimalApiCustomAuthenticationHandlers.Contracts;
//
// namespace MinimalApiCustomAuthenticationHandlers.Authorization;
//
// /// <summary>
// /// Represents the custom <see cref="HasPermissionAuthorizationHandler"/> class.
// /// </summary>
// internal class HasPermissionAuthorizationHandler : AttributeAuthorizationHandler<
//     HasPermissionAuthorizationRequirement, HasPermissionAttribute>
// {
//     private readonly HttpContext _httpContext;
//
//
//     /// <summary>
//     /// Initializes a new instance of <see cref="HasPermissionAuthorizationHandler"/>
//     /// </summary>
//     /// <remarks> Normally you would inject your dependencies via the constructor</remarks>
//     public HasPermissionAuthorizationHandler(HttpContext httpContext)
//     {
//         _httpContext = httpContext;
//     }
//     
//     /// <inheritdoc />
//     protected override async Task HandleRequirementAsync(
//         AuthorizationHandlerContext context,
//         HasPermissionAuthorizationRequirement requirement,
//         HasPermissionAttribute attribute)
//     {
//         var permissions = _httpContext.User?.Claims
//             .Select(x => new UserPermissionDto(x.Value))
//             .ToList();
//         
//         
//         if (!await AuthorizeAsync(attribute.Name, permissions))
//         {
//             context.Fail();
//             return;
//         }
//
//         context.Succeed(requirement);
//     }
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="permission">The required permission to access the endpoint.</param>
//     /// <param name="permissions">The user permissions to check against.</param>
//     /// <returns>True is authorization succeeded, false otherwise.</returns>
//     private Task<bool> AuthorizeAsync(string permission, List<UserPermissionDto> permissions)
//     {
// #if !DEBUG
//             return Task.FromResult(true);
// #endif
//         return Task.FromResult(permissions.Any(x => x.Code == permission));
//     }
// }