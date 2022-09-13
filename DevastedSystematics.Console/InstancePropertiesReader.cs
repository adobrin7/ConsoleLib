using System.Reflection;
using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib;

public sealed class InstancePropertiesReader
{
    public InstancePropertiesReader(object instance, string nameSeparator = "", IPropertyNameFormatter? nameFormatter = null)
    {
        IEnumerable<PropertyInfo>? publicSetters = instance
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetSetMethod() is not null and { IsPublic: true });

        propertiesToRead = publicSetters
            .Select(info => new DialogProperty(
                instance,
                info,
                new DialogPropertyContext(
                    info,
                    nameFormatter)))
            .Where(property => property.Context.CanRead);
        this.nameSeparator = nameSeparator;
        this.nameFormatter = nameFormatter;
    }

    private readonly IEnumerable<DialogProperty> propertiesToRead;
    private readonly string nameSeparator;
    private readonly IPropertyNameFormatter? nameFormatter;

    public bool IsDialogCanceled { get; private set; } = false;

    public bool CheckPropertyType { get; set; } = false;
    public bool IsRecursive { get; set; } = false;
    public string? DialogCancelationToken { get; set; }
    public string? PropertySkipToken { get; set; }

    public event Action? OnCanceled;
    public event Action? OnSkipped;
    public event Func<DialogProperty, bool>? OnInvalidType;

    public void ReadInDialog()
    {
        foreach (var property in propertiesToRead)
        {
            if (IsDialogCanceled)
            {
                OnCanceled?.Invoke();
                return;
            }
            if (IsRecursive)
                ReadNestedInstance(property);
            IsDialogCanceled = StartDialogForProperty(property);
        }
    }

    private void ReadNestedInstance(DialogProperty property)
    {
        if (property.GetType().IsClass)
        {
            var reader = new InstancePropertiesReader(property, nameSeparator, nameFormatter);
            reader.ReadInDialog();
            IsDialogCanceled = reader.IsDialogCanceled;
        }
    }

    private bool StartDialogForProperty(DialogProperty property)
    {
        Write(property.Context.Name + nameSeparator);
        string? input = ReadLine();
        if (string.Equals(input, DialogCancelationToken, StringComparison.OrdinalIgnoreCase))
            return true;
        if (!CanSkipProperty(input, property))
            SetInputAsPropertyValue(input, property);
        else
            OnSkipped?.Invoke();
        return false;
    }

    private bool CanSkipProperty(string? input, DialogProperty property) =>
        string.Equals(input, PropertySkipToken, StringComparison.OrdinalIgnoreCase) &&
        !property.Context.IsRequired;

    private void SetInputAsPropertyValue(string? input, DialogProperty property)
    {
        var context = property.Context;
        if (!IsInputMatchesPropertyRequierments(input, property))
        {
            context.InvalidPatternMessage.Show();
            input = Input.ReadUntilValid(context.ReadPattern!, context.Name + nameSeparator, context.InvalidPatternMessage);
        }
        TrySetPropertyValue(property, input!);
    }

    private bool IsInputMatchesPropertyRequierments(string? input, DialogProperty property)
    {
        return input is not null && (property.Context.ReadPattern is null ||
            Regex.IsMatch(input, property.Context.ReadPattern));
    }

    private void TrySetPropertyValue(DialogProperty property, string value)
    {
        try
        {
            property.SetTypedValue(value);
        }
        catch (FormatException) when (CheckPropertyType || OnInvalidType is not null)
        {
            bool isHandeled = OnInvalidType?.Invoke(property) ?? false;
            if (!isHandeled)
                property.SetDefaultValue();
        }
    }
}
