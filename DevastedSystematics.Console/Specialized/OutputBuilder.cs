using System.Text;

namespace DevastedSystematics.Console.Specialized;

public class OutputBuilder : IFormattable
{
    public OutputBuilder()
    {
    }

    public OutputBuilder(string text) : this(text, ConsoleColor.Gray)
    {
    }

    public OutputBuilder(string text, ConsoleColor ConsoleColor, bool clearBeforeShow = false)
    {
        stringBuilder.Append(text);
        Color = ConsoleColor;
        ClearBeforeShow = clearBeforeShow;
    }

    private readonly StringBuilder stringBuilder = new();

    public ConsoleColor Color { get; init; } = ConsoleColor.Gray;
    public TextAlignment TextAlignment { get; init; } = TextAlignment.Begin;
    public bool ClearBeforeShow { get; set; } = false;

    public OutputBuilder Append(string text)
    {
        stringBuilder.Append(text);
        return this;
    }

    public OutputBuilder AppendFormatted(FormattableString text, IFormatProvider? formatProvider = null)
    {
        stringBuilder.Append(text.ToString(formatProvider));
        return this;
    }

    public OutputBuilder AppendLine(string text, int linesCount = 1)
    {
        Append(text + GetNewLines(linesCount));
        return this;
    }

    private string GetNewLines(int count) =>
        string.Concat(Enumerable.Repeat(Environment.NewLine, count));

    public OutputBuilder AppendLineFormatted(FormattableString text, int linesCount = 1,
        IFormatProvider? formatProvider = null)
    {
        stringBuilder.Append(text.ToString(formatProvider) + GetNewLines(linesCount));
        return this;
    }

    public void Show()
    {
        CursorPosition.SetForText(stringBuilder.ToString(), TextAlignment);
        ForegroundColor = Color;
        Write(stringBuilder);
        ResetColor();
    }

    public override string ToString() =>
        stringBuilder.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        format is null ?
        ToString() :
        string.Format(formatProvider, format, stringBuilder.ToString());

    public static OutputBuilder operator +(OutputBuilder current, OutputBuilder other) =>
        new OutputBuilder(current.ToString() + other.ToString());
}