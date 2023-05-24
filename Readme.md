# Using Custom Authentication Handlers In Minimal-API

## Todo

- Ensure that all endpoints use our custom attributes by using reflection to check for the presence of either the HasPermissionAttribute or HasNoPermissionAttribute attribute. If an endpoint does not have one of these attributes, throw an exception to prevent the application from starting.

## The Context 
For some apps that has dozens if not hundreds of endpoint, it is quite impossible to create a separate 
authorization policy for each minimal endpoint in order to have that fine grained access policy. 

The idea basically is quite simple, just register a single policy then decorate our endpoints with 
``HasPermissionAttribute``. <br>
> That by default will pass the policy we created to the **AuthorizeAttribute(string policy)** 
constructor implicitly. <br>

> The **Name** field of the **HasPermissionAttribute** will be used to pass the name of the actual permission
to access a given endpoint.

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

Then create and register a ``CustomAuthorizationRequirement()`` in the policy we created. <br>
By default it needs to inherit from ``AuthorizationHandler<TRequirement>`` class and the ``IAuthorizationRequirement`` interface.<br>
Use the ``CustomAuthorizationRequirement()`` class to define your custom logic to either succeed of fail an authorization.
<br>
Implementation as follows:

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

And finally add some extension methods to be able to use something like ``app.Map("",...).RequirePermission()``
instead of ``app.Map("",...).RequireAuthorization()``.


