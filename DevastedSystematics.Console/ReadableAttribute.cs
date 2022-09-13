namespace DevastedSystematics.ConsoleLib;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ReadableAttribute : Attribute
{
    public string? Name { get; init; }

    public string? SkipToken { get; init; }

    public string? CancelToken { get; init; }

    public object? DefaultValue { get; init; } // todo generic
}
