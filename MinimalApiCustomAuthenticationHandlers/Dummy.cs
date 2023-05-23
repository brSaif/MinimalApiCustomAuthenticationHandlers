/// <summary>
/// Represents a dummy minimal-api routing group.
/// </summary>
public static class Dummy
{
    /// <summary>
    /// Maps The 'Dummy' routing group and its subsequent endpoints.
    /// </summary>
    /// <param name="builder">The route builder.</param>
    /// <returns>Route group builder.</returns>
    public static RouteGroupBuilder MapDummy(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/hell").RequireAuthorization();

        group.MapGet("/dente", 
                () => "ok")
            .RequirePermission(UserPermissions.AcceptMyGreetings);
        
        group.MapGet("/Abu-al-alaa", 
                () => "ok")
            .RequireNoPermission();

        return group;
    }
}