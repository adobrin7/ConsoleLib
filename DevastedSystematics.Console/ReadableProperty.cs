using System.Reflection;
using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib;

public sealed class ReadableProperty
{
    public ReadableProperty(object containingInstance, PropertyInfo info)
    {
        readableAttribute = info.GetCustomAttribute<ReadableAttribute>();
        validAttribute = info.GetCustomAttribute<ValidatedOnReadingAttribute>();
        isRequired = info.GetCustomAttribute<RequieredReadingAttribute>() is not null;
        originalName = info.Name;
        property = new Property(containingInstance, info);
        CanRead = info.GetCustomAttribute<IgnoreReadingAttribute>() is null;
    }

    private readonly ReadableAttribute? readableAttribute;

    private readonly ValidatedOnReadingAttribute? validAttribute;

    private readonly bool isRequired;

    private readonly string originalName;

    private readonly Property property;

    internal bool CanRead { get; }

    public Message InvalidPatternMessage => new(
        validAttribute?.ErrorMessage ?? "",
        validAttribute?.MessageColor ?? ConsoleColor.Gray);

    public object? Value => property.Value;

    public bool StartReading(IPropertyNameFormatter? nameFormatter = null)
    {
        string nameToShow = GetFormattedName(nameFormatter);
        string? input = ReadLine();
        
        if (IsReadingCanceled(input))
            return true;
        if(IsReadingSkipped(input))
            return false;

        if (!IsValidInput(input))
        {
            InvalidPatternMessage.Show();
            Input.ReadUntilValid(validAttribute?.Pattern!, nameToShow, InvalidPatternMessage);
        }

        property.Value = input;

        return false;
    }

    private string GetFormattedName(IPropertyNameFormatter? nameFormatter = null)
    {
        string nameToShow = readableAttribute?.Name ?? originalName;
        return nameFormatter?.Format(nameToShow) ?? nameToShow;
    }

    private bool IsReadingCanceled(string? input) =>
        readableAttribute?.CancelToken is not null &&
        string.Equals(input, readableAttribute.CancelToken, StringComparison.OrdinalIgnoreCase);

    private bool IsReadingSkipped(string? input) =>
        readableAttribute?.SkipToken is not null &&
        !isRequired &&
        string.Equals(input, readableAttribute.SkipToken, StringComparison.OrdinalIgnoreCase);

    private bool IsValidInput(string? input) =>
        input is not null && 
        (validAttribute?.Pattern is null || Regex.IsMatch(input, validAttribute.Pattern));

    public void SetDefaultValue()
    {
        if (readableAttribute?.DefaultValue is null)
        {
            property.SetDefaultValue();
            return;
        }
        property.Value = readableAttribute?.DefaultValue;
    }
}