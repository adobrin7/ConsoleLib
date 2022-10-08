using System.Reflection;

namespace DevastedSystematics.ConsoleLib;

public sealed class PropertiesReader
{
    internal PropertiesReader(object instance, IPropertyNameFormatter? nameFormatter = null, 
        bool checkTypes = true)
    {
        propertiesToRead = instance
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .AsParallel()
            .AsOrdered()
            .Where(property => property.GetSetMethod() is not null and { IsPublic: true })
            .Select(info => new ReadableProperty(instance, info))
            .Where(property => property.CanRead);
        this.nameFormatter = nameFormatter;
        this.checkTypes = checkTypes;
    }

    private readonly IEnumerable<ReadableProperty> propertiesToRead;

    private readonly IPropertyNameFormatter? nameFormatter;

    private readonly bool checkTypes;

    internal bool IsDialogCanceled { get; private set; } = false;

    public static bool StartReadingFor(
        object instance, 
        IPropertyNameFormatter? nameFormatter = null)
    {
        PropertiesReader reader = new PropertiesReader(instance, nameFormatter);
        reader.Read();
        return reader.IsDialogCanceled;
    }

    public static bool StartReadingFor<TInstance>(
        out TInstance instance,
        IPropertyNameFormatter? nameFormatter = null)
    {
        instance = (TInstance)Activator.CreateInstance(typeof(TInstance))!;
        PropertiesReader reader = new PropertiesReader(instance, nameFormatter);
        reader.Read();
        return reader.IsDialogCanceled;
    }

    public static bool StartUncheckedReadingFor(
        object instance, 
        IPropertyNameFormatter? nameFormatter = null)
    {
        PropertiesReader reader = new PropertiesReader(instance, nameFormatter, false);
        reader.Read();
        return reader.IsDialogCanceled;
    }

    private void Read()
    {
        foreach (var property in propertiesToRead)
        {
            if (IsDialogCanceled)
                return;            
            if (CanReadNestedInstance(property))
            {
                ReadNestedInstance(property);
                continue;
            }
            IsDialogCanceled = TryReadProperty(property);
        }
    }

    private bool CanReadNestedInstance(ReadableProperty property) =>
        property.Value is not null &&
        property.Value.GetType().IsClass;

    private void ReadNestedInstance(ReadableProperty property)
    {
        ArgumentNullException.ThrowIfNull(property.Value, nameof(property.Value));
        PropertiesReader reader = new PropertiesReader(property.Value);
        reader.Read();
        IsDialogCanceled = reader.IsDialogCanceled;
    }

    private bool TryReadProperty(ReadableProperty property)
    {
        try
        {
            return property.StartReading(nameFormatter);
        }
        catch (FormatException) when (checkTypes)
        {
            property.SetDefaultValue();
            return false;
        }
    }
}
