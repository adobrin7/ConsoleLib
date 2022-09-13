using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib.Specialized;

public static class Dialog
{
    public static bool Edit(ReadableValue value, string cancelOn = "c", string skipOn = "")
    {
        var (requestMessage, pattern, invalidPatternMessage) = value;

        while (true)
        {
            requestMessage.Show();
            string input = ReadLine() ?? "";

            if (string.Equals(input, cancelOn, StringComparison.OrdinalIgnoreCase))
            {
                value.OnCancel?.Invoke();
                return true;
            }
            if (string.Equals(input, skipOn, StringComparison.OrdinalIgnoreCase))
            {
                value.OnSkip?.Invoke();
                continue;
            }
            if (Regex.IsMatch(input, pattern))
            {
                value.OnRead?.Invoke(input);
                break;
            }
            invalidPatternMessage.Show();
        }
        return false;
    }

    public static bool Edit(ReadableValue[] values, string cancelOn = "c", string skipOn = "") =>
        Edit(values.AsSpan(), cancelOn, skipOn);

    public static bool Edit(ReadOnlySpan<ReadableValue> values, string cancelOn = "c", string skipOn = "")
    {
        foreach (var value in values)
        {
            bool isCanceled = Edit(value, cancelOn, skipOn);
            if (isCanceled)
                return true;
        }
        return false;
    }

    public static bool Create(ReadableValue value, string cancelOn = "c")
    {
        var (requestMessage, pattern, invalidPatternMessage) = value;

        while (true)
        {
            requestMessage.Show();
            string input = ReadLine() ?? "";

            if (string.Equals(input, cancelOn, StringComparison.OrdinalIgnoreCase))
            {
                value.OnCancel?.Invoke();
                return true;
            }
            if (Regex.IsMatch(input, pattern))
            {
                value.OnRead?.Invoke(input);
                break;
            }
            invalidPatternMessage.Show();
        }
        return false;
    }

    public static bool Create(ReadableValue[] values, string cancelOn = "c") =>
        Create(values.AsSpan(), cancelOn);

    public static bool Create(ReadOnlySpan<ReadableValue> values, string cancelOn = "c")
    {
        foreach (var value in values)
        {
            bool isCanceled = Create(value, cancelOn);
            if (isCanceled)
                return true;
        }
        return false;
    }
}
