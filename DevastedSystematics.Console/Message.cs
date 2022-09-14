namespace DevastedSystematics.ConsoleLib;

public class Message
{
    public Message(string text = "", ConsoleColor color = ConsoleColor.Gray) => 
        (Text, Color) = (text, color);

    public static Message Empty { get; } = new();

    public string Text { get; }

    public ConsoleColor Color { get; }

    protected internal virtual void Show() => Output.Show(Text, Color);
}