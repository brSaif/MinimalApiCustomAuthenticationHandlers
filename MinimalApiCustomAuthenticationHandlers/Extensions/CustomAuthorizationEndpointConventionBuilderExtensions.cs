using Microsoft.AspNetCore.Authorization;

public static class CustomAuthorizationEndpointConventionBuilderExtensions {
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
    
    public static TBuilder RequireNoPermission<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new NoPermissionRequiredAttribute());
        });
        return builder;
    }
}