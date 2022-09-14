namespace DevastedSystematics.ConsoleLib;

public abstract class CrudOptionDialogMenu : OptionDialogMenu<CrudOptionDialogMenu.Option>
{
    sealed protected override Action? SelectOptionHandler(Option selectedOption) => selectedOption switch
    {
        Option.Create => CreateOptionHandler,
        Option.Read => ReadOptionHandler,
        Option.Update => UpdateOptionHandler,
        Option.Delete => DeleteOptionHandler,
        Option.Stop => OnStop,
        _ => null,
    };

    protected abstract void CreateOptionHandler();

    protected abstract void ReadOptionHandler();

    protected abstract void UpdateOptionHandler();

    protected abstract void DeleteOptionHandler();

    public virtual event Action? OnStop;

    public enum Option
    {
        None,
        Create,
        Read,
        Update,
        Delete,
        Stop,
    }
}