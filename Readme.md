# Using Custom Authentication Handlers In Minimal-API

## Todo

- Ensure that all endpoints use our custom attributes by using reflection to check for the presence of either the HasPermissionAttribute or HasNoPermissionAttribute attribute. If an endpoint does not have one of these attributes, throw an exception to prevent the application from starting.

## The Context 
Inspired by Anthon and Nick Chapsas' YouTube videos on how to extend/implement custom types in .NET, I created a custom authorization handler for ASP.NET Core minimal APIs.
<br>
<br>
This is useful for applications with dozens or hundreds of api endpoints, as it allows for fine-grained access control without the need to create a separate authorization policy for each endpoint.
<br>
<br>
The idea basically is quite simple, just register a single access policy then decorate our endpoints with the ``HasPermissionAttribute`` custom attribute. <br>
The ``HasPermissionAttribute`` class is used to control access to an api endpoint based on the user's permissions. It passes the name of the created policy to the ``AuthorizeAttribute(string policy)`` base constructor, which sets the policy to use as part of the authorization process. It also defines a **Name** property, which will hold the name of the permission required to access a given endpoint.
<br>
<br>

<pre>
/// Represents the <see cref="HasPermissionAttribute"/> Authorize Attribute.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public string Name { get; }
    internal const string HasPermissionPolicyName = "HasPermission";

    public HasPermissionAttribute(string name)
        : base(HasPermissionPolicyName)
    {
        Name = name;
    }
}
</pre>
<br>

Next, create and register a ``CustomAuthorizationRequirement`` in the policy we created. By default, it must inherit from the ``AuthorizationHandler<TRequirement>`` class and the ``IAuthorizationRequirement`` interface. <br>
Use the ``CustomAuthorizationRequirement<>`` class to define the custom logic to either succeed or fail an authorization, in our case we check if the user has the permission required to access a given endpoint otherwise fail the authorization.
<br>
<br>
Here is an example implementation:

<pre>
    public class HasPermissionAuthorizationRequirement 
        : AuthorizationHandler<HasPermissionAuthorizationRequirement>, IAuthorizationRequirement
    { }
</pre>


<pre>
o.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
    {
        policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
        policyBuilder.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
    });
</pre>

<br>
<br>
Finally, add some extension methods to be able to use something like ``app.Map("",...).RequirePermission("PermissionName")`` instead of ``app.Map("",...).RequireAuthorization()``


