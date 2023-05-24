using Microsoft.AspNetCore.Authorization;
using MinimalApiCustomAuthenticationHandlers.Authorization;

namespace MinimalApiCustomAuthenticationHandlers.Extensions;

/// <summary>
/// Represents the <see cref="CustomAuthorizationEndpointConventionBuilderExtensions"/> class.
/// </summary>
public static class CustomAuthorizationEndpointConventionBuilderExtensions {
    
    /// <summary>
    /// By default it adds a custom authorization policy named <c>HasPermission</c> through the <see cref="HasPermissionAttribute"/> class. 
    /// </summary>
    /// <param name="builder">The endpoint convention builder</param>
    /// <param name="policyNames">The list of permissions required to access an endpoint</param>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder</typeparam>
    /// <returns>The endpoint convention builder</returns>
    /// <exception cref="ArgumentNullException">When the 'builder' and/or 'policyNames' are null </exception>
    /// <seealso cref="HasPermissionAttribute"/>
    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder, params string[] policyNames) 
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (policyNames == null)
        {
            throw new ArgumentNullException(nameof(policyNames));
        }

        return builder.RequireAuthorization(policyNames.Select(p => new HasPermissionAttribute(p)).ToArray());
    }
    
    /// <summary>
    /// Adds authorization policies with the specified <see cref="IAuthorizeData"/> to the endpoint(s).
    /// </summary>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="authorizeData">
    /// A collection of <paramref name="authorizeData"/>. If empty, the default authorization policy will be used.
    /// </param>
    /// <returns>The original convention builder parameter.</returns>
    public static TBuilder RequireAuthorization<TBuilder>(this TBuilder builder, params IAuthorizeData[] authorizeData)
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (authorizeData == null)
        {
            throw new ArgumentNullException(nameof(authorizeData));
        }

        if (authorizeData.Length == 0)
        {
            authorizeData = new IAuthorizeData[] { new AuthorizeAttribute(), };
        }

        RequireAuthorizationCore(builder, authorizeData);
        return builder;
    }
    
    
    private static void RequireAuthorizationCore<TBuilder>(TBuilder builder, IEnumerable<IAuthorizeData> authorizeData)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
        {
            foreach (var data in authorizeData)
            {
                endpointBuilder.Metadata.Add(data);
            }
        });
    }
    
    
    /// <summary>
    /// Allows anonymous access to the endpoint by adding <see cref="NoPermissionRequiredAttribute"/>
    /// to the endpoint metadata. This will bypass all authorization checks for the endpoint
    /// including the default authorization policy and fallback authorization policy
    /// </summary>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <typeparam name="TBuilder">The original convention builder.</typeparam>
    /// <returns></returns>
    public static TBuilder RequireNoPermission<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new NoPermissionRequiredAttribute());
        });
        return builder;
    }
}