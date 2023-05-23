using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Represents the <see cref="HasPermissionAttribute"/> Authorize Attribute.
/// </summary>
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