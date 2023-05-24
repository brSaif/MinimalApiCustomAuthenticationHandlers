namespace MinimalApiCustomAuthenticationHandlers.Contracts;


internal class UserPermissionDto
{
    public UserPermissionDto(string permission)
    {
        Code = permission;
    }
    public string Code { get; set; }
}
