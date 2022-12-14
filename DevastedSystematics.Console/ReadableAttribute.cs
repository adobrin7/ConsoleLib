namespace DevastedSystematics.ConsoleLib;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ReadableAttribute : Attribute
{
    public string? Name { get; init; }

    public object? DefaultValue { get; init; } // todo generic in .net 7
}
