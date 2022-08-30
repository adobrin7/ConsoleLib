namespace DevastedSystematics.Console;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DialogAttribute : Attribute
{
    public string? Name { get; init; }
    public object? DefaultValue { get; init; } // todo generic
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ValidAttribute : Attribute
{
    public ValidAttribute(string pattern)
    {
        Pattern = pattern;
    }

    public string Pattern { get; }
    public string ErrorMessage { get; init; } = string.Empty;
    public ConsoleColor MessageColor { get; set; } = ConsoleColor.DarkRed;
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class RequieredAttribute : Attribute
{

}