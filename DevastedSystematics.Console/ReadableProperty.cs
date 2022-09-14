using System.Reflection;
using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib;

internal sealed class ReadableProperty
{
    public ReadableProperty(object containingInstance, PropertyInfo info)
    {
        instanceLevelAttribute = containingInstance.GetType()
            .GetCustomAttribute<InterruptableReadingAttribute>();
        property = new Property(containingInstance, info);
        readableAttribute = info.GetCustomAttribute<ReadableAttribute>();
        validAttribute = info.GetCustomAttribute<ValidatedOnReadingAttribute>();
        isRequired = info.GetCustomAttribute<RequieredReadingAttribute>() is not null;
        originalName = info.Name;
        invalidPatternMessage = new(
            validAttribute?.ErrorMessage ?? "",
            validAttribute?.MessageColor ?? ConsoleColor.Gray);
        CanRead = info.GetCustomAttribute<IgnoreReadingAttribute>() is null;
    }

    private readonly InterruptableReadingAttribute? instanceLevelAttribute;

    private readonly Property property;

    private readonly ReadableAttribute? readableAttribute;

    private readonly ValidatedOnReadingAttribute? validAttribute;

    private readonly bool isRequired;

    private readonly string originalName;

    private readonly Message invalidPatternMessage;

    public bool CanRead { get; }

    public object? Value => property.Value;

    public bool StartReading(IPropertyNameFormatter? nameFormatter = null)
    {
        string nameToShow = GetFormattedName(nameFormatter);
        Write(nameToShow);

        string? input = ReadLine();
        
        if (IsReadingCanceled(input))
            return true;
        if(IsReadingSkipped(input))
            return false;

        if (!IsValidInput(input))
        {
            invalidPatternMessage.Show();
            Input.ReadUntilValid(validAttribute?.Pattern!, nameToShow, invalidPatternMessage);
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
        instanceLevelAttribute?.CancelToken is not null &&
        string.Equals(
            input, 
            instanceLevelAttribute.CancelToken, 
            StringComparison.OrdinalIgnoreCase);

    private bool IsReadingSkipped(string? input) =>
        instanceLevelAttribute is not null &&
        !isRequired &&
        string.Equals(
            input, 
            instanceLevelAttribute.SkipToken, 
            StringComparison.OrdinalIgnoreCase);

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