using System.Reflection;

namespace DevastedSystematics.ConsoleLib;

public class DialogProperty
{
    internal DialogProperty(object containingInstance, PropertyInfo info, DialogPropertyContext context)
    {
        this.containingInstance = containingInstance;
        this.info = info;
        Context = context;
    }

    private readonly object containingInstance;
    private readonly PropertyInfo info;

    public DialogPropertyContext Context { get; }

    public void SetTypedValue(object value)
    {
        if (!Context.CanRead)
            throw new InvalidOperationException(
                string.Intern($"Property with {nameof(NonDialogAttribute)} can not be assigned."));
        var typedValue = Convert.ChangeType(value, info.PropertyType);
        info.SetValue(containingInstance, typedValue);
    }

    public void SetDefaultValue()
    {
        if (!Context.CanRead)
            throw new InvalidOperationException(
                string.Intern($"Property with {nameof(NonDialogAttribute)} can not be assigned."));

        if (Context.DefaultValue is null)
            info.SetValue(containingInstance, Activator.CreateInstance(info.PropertyType));
        else
            info.SetValue(containingInstance, Context.DefaultValue);
    }
}
