namespace DevastedSystematics.ConsoleLib;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ValidatedOnReadingAttribute : Attribute
{
    public ValidatedOnReadingAttribute(string pattern) => Pattern = pattern;

    public string Pattern { get; }

    public string ErrorMessage { get; init; } = string.Empty;

    public ConsoleColor MessageColor { get; init; } = ConsoleColor.DarkRed;
}
