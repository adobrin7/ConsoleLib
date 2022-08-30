namespace DevastedSystematics.Console;

public abstract class DialogMenu
{
    public IEnumerable<Message> QuestionMessages { private protected get; init; } =
        Enumerable.Empty<Message>();
    public string MessagesSeparator { private get; init; } = string.Empty;

    private Consequence AnswerEffect { get; set; } = Consequence.Continue;

    public void Start()
    {
        while (AnswerEffect is Consequence.Continue)
        {
            Show();
            AnswerEffect = Answer();
        }
    }

    private void Show()
    {
        foreach (var message in QuestionMessages)
        {
            message.Show();
            Write(MessagesSeparator);
        }
    }

    protected abstract Consequence Answer();

    protected enum Consequence
    {
        Stop,
        Continue,
    }
}
