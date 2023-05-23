var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
    

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
    {
        policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
        // Todo: add the auth. scheme
        policyBuilder.AddAuthenticationSchemes();
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapDummy();

app.Run();