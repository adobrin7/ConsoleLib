namespace DevastedSystematics.ConsoleLib;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class InterruptableReadingAttribute : Attribute
{
    public string SkipToken { get; init; } = "";

    public string CancelToken { get; set; } = "c";
}