using System.Reflection;

namespace DevastedSystematics.ConsoleLib;

public sealed class Property
{
    internal Property(object containingInstance, PropertyInfo info)
    {
        this.containingInstance = containingInstance;
        this.info = info;
    }

    private readonly object containingInstance;

    private readonly PropertyInfo info;

    public object? Value
    {
        get => info.GetValue(containingInstance);
        set => info.SetValue(
            containingInstance,
            Convert.ChangeType(value, info.PropertyType));
    }

    public void SetDefaultValue() =>
        info.SetValue(containingInstance, Activator.CreateInstance(info.PropertyType));
}
