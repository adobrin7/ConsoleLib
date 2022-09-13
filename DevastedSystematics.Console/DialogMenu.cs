namespace DevastedSystematics.ConsoleLib;

public abstract class DialogMenu
{
    public List<Message> QuestionMessages { private protected get; init; } =
        new List<Message>(capacity: 0);

    public Message SeparatingMessage { private get; init; } = Message.Empty;

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
        int count = 0;
        foreach (var message in QuestionMessages)
        {
            message.Show();
            if (count < QuestionMessages.Count - 1)
                SeparatingMessage.Show();
            count++;
        }
    }

    protected abstract Consequence Answer();

    protected enum Consequence
    {
        Stop,
        Continue,
    }
}
