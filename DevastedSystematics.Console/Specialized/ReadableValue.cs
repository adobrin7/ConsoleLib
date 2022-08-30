namespace DevastedSystematics.Console.InOut.Specialized;

public sealed class ReadableValue
{
    public ReadableValue(ValueRequest valueRequest, Action<string> onRead)
        => (Request, OnRead) = (valueRequest, onRead);

    private ValueRequest Request { get; }
    internal Action<string> OnRead { get; }
    public Action? OnSkip { internal get; init; }
    public Action? OnCancel { internal get; init; }

    public void Deconstruct(out Message requestMessage, out string pattern, out Message invalidPatternMessage)
    {
        requestMessage = Request.RequestMessage;
        pattern = Request.Pattern;
        invalidPatternMessage = Request.InvalidPatternMessage;
    }
}