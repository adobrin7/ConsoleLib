namespace DevastedSystematics.Console;

/// <summary>
/// Base class implementing dialog based on console menu, where every input is considered to be option. Valid option, by convension, is sequence number corresponding to the message sequence number.
/// </summary>
/// <typeparam name="TOption">Specifies supported options. By convention, first option reserved for lack of option, and last option reserved for dialog stopping option.</typeparam>
public abstract class OptionDialogMenu<TOption> : DialogMenu
    where TOption : struct, Enum
{
    public OptionDialogMenu()
    {
        MessagesSeparator = "\n";
        NoneOptionByConvention = SupportedOptions[0];
        StopOptionByConvention = SupportedOptions[SupportedOptions.Length - 1];
    }

    public Message BeforeInputMessage { private get; init; } = Message.Empty;
    public Message InvalidOptionMessage { private get; init; } = Message.Empty;

    private TOption[] SupportedOptions => Enum.GetValues<TOption>();
    private readonly TOption NoneOptionByConvention;
    private readonly TOption StopOptionByConvention;

    sealed protected override Consequence Answer()
    {
        string input = GetInput();
        TOption selectedOption = Enum.Parse<TOption>(input, ignoreCase: true);
        return Handle(selectedOption);
    }

    private string GetInput()
    {
        string optionPattern = $"^[0-{QuestionMessages.Count()}]$";
        return Input.ReadUntilValid(optionPattern, BeforeInputMessage, InvalidOptionMessage);
    }

    private Consequence Handle(TOption option)
    {
        if (option.Equals(NoneOptionByConvention))
            return Consequence.Stop;

        Action? handler = SelectOptionHandler(option);
        if (option.Equals(StopOptionByConvention))
            return Stop(handler);

        return Invoke(handler);
    }

    protected abstract Action? SelectOptionHandler(TOption option);

    private Consequence Stop(Action? optionalHandler)
    {
        optionalHandler?.Invoke();
        return Consequence.Stop;
    }

    private Consequence Invoke(Action? handler)
    {
        if (handler is null)
            throw new ArgumentOutOfRangeException(nameof(handler), $"Selected option is out of {nameof(TOption)} range or missing handler. Check that the {nameof(TOption)} enum follows the convention and messages count is not greater then provided options count in {nameof(TOption)} enum. Or, check if null is returned instead of handler.");
        handler();
        return Consequence.Continue;
    }
}