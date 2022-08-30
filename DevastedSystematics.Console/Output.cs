namespace DevastedSystematics.Console;

public static class Output
{
    public static void Show(string text, ConsoleColor ConsoleColor = ConsoleColor.Gray, TextAlignment alignment = TextAlignment.Begin)
    {
        CursorPosition.SetForText(text, alignment);
        ForegroundColor = ConsoleColor;
        Write(text);
        ResetColor();
    }

    public static void ShowLine(string text, ConsoleColor ConsoleColor = ConsoleColor.Gray, int linesCount = 1, TextAlignment alignment = TextAlignment.Begin)
    {
        string line = text + new string('\n', linesCount);
        Show(line, ConsoleColor, alignment);
    }

    public static void ClearThenShow(string text, ConsoleColor ConsoleColor = ConsoleColor.Gray, TextAlignment alignment = TextAlignment.Begin)
    {
        Clear();
        Show(text, ConsoleColor, alignment);
    }

    public static void ClearThenShowLine(string text, ConsoleColor ConsoleColor = ConsoleColor.Gray, int linesCount = 1, TextAlignment alignment = TextAlignment.Begin)
    {
        Clear();
        ShowLine(text, ConsoleColor, linesCount, alignment);
    }
}
