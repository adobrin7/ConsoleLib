namespace DevastedSystematics.ConsoleLib;

public static class CursorPosition
{
    public static void SetForText(string text, TextAlignment alignment)
    {
        if (alignment is TextAlignment.Begin || text.Length > WindowWidth)
            return;
        if (alignment is TextAlignment.Middle)
        {
            AlignMidle(text);
            return;
        }
        AlignEnd(text);
    }

    private static void AlignMidle(string text)
    {
        int leftOffset = WindowWidth / 2 - text.Length / 2;
        if (CursorLeft > leftOffset)
            CursorTop += 1;
        CursorLeft = leftOffset;
    }

    private static void AlignEnd(string text) => CursorLeft = WindowWidth - text.Length;
}
