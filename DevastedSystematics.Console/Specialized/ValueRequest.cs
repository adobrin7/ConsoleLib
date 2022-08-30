namespace DevastedSystematics.Console.InOut.Specialized;

public sealed class ValueRequest
{
    // TODO: Required properties in .NET 7
    public Message RequestMessage { get; init; }
    public string Pattern { get; init; }
    public Message InvalidPatternMessage { get; init; }
}