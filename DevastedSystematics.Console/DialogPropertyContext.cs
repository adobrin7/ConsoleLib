using System.Reflection;

namespace DevastedSystematics.Console;

public sealed record class DialogPropertyContext
{
    internal DialogPropertyContext(PropertyInfo info) : this(info, null)
    {

    }

    internal DialogPropertyContext(PropertyInfo info, IPropertyNameFormatter? nameFormatter)
    {
        commonContext = info;
        customContext = new PropertyCustomContextMarshaller(info);
        this.nameFormatter = nameFormatter;
    }

    private readonly PropertyInfo commonContext;
    private readonly PropertyCustomContextMarshaller customContext;
    private readonly IPropertyNameFormatter? nameFormatter;

    internal bool CanRead => customContext.CanRead;
    internal bool IsRequired => customContext.IsRequired;
    public string Name
    {
        get => (customContext.Name, nameFormatter) switch
        {
            var (name, _) when name is not null => name,
            var (_, formatter) when formatter is not null => formatter.Format(commonContext.Name),
            (_, _) => commonContext.Name,
        };
    }
    public string? ReadPattern => customContext.ReadPattern;
    public Message InvalidPatternMessage => customContext.InvalidPatternMessage;
    public object? DefaultValue => customContext.DefaultValue;

    private record PropertyCustomContextMarshaller
    {
        public PropertyCustomContextMarshaller(PropertyInfo info)
        {
            CanRead = info.GetCustomAttribute<NonDialogAttribute>() is null;
            IsRequired = info.GetCustomAttribute<RequieredAttribute>() is not null;
            dialogAttribute = info.GetCustomAttribute<DialogAttribute>();
            validAttribute = info.GetCustomAttribute<ValidAttribute>();
        }

        private readonly DialogAttribute? dialogAttribute;
        private readonly ValidAttribute? validAttribute;

        internal bool CanRead { get; }
        internal bool IsRequired { get; }
        public string? Name => dialogAttribute?.Name;
        public object? DefaultValue => dialogAttribute?.DefaultValue;
        public string? ReadPattern => validAttribute?.Pattern;
        public Message InvalidPatternMessage =>
            new(validAttribute?.ErrorMessage ?? "",
                validAttribute?.MessageColor ?? ConsoleColor.Gray);
    }
}