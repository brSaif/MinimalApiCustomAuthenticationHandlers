namespace MinimalApiCustomAuthenticationHandlers.Endpoints.Dummy;

/// <summary>
/// Represents a Dummy permission to check against.
/// </summary>
public static class DummyPermissions
{
    /// <summary>
    /// Returns the <see cref="AcceptMyGreetings"/> permission.
    /// </summary>
    public const string AcceptMyGreetings = "AcceptMyGreetings";
    
    /// <summary>
    /// Returns the <see cref="GreetMeBack"/> permission.
    /// </summary>
    public const string GreetMeBack = "GreetMeBack";
    
    /// <summary>
    /// Returns the <see cref="MakeSillyjokes"/> permission.
    /// </summary>
    public const string MakeSillyJokes = "MakeSillyJokes";
}