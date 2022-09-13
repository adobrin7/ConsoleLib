namespace DevastedSystematics.ConsoleLib;

public static class InstanceReader
{
    public static void ReadInDialog<T>(T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        new InstancePropertiesReader(instance, ": ", new PropertyNamesToRegularCaseFormatter())
            .ReadInDialog();
    }

    public static bool StartCreatingDialog<T>(T instance, string cancelationToken = "c") 
        where T : class
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        var reader = new InstancePropertiesReader(instance, ": ", 
            new PropertyNamesToRegularCaseFormatter())
        {
            DialogCancelationToken = cancelationToken,
            IsRecursive = true,
        };
        reader.ReadInDialog();
        return reader.IsDialogCanceled;
    }

    public static bool StartEditingDialog<T>(T instance, string cancelationToken = "c", 
        string skipToken = "")
        where T : class
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        var reader = new InstancePropertiesReader(instance, ": ", new PropertyNamesToRegularCaseFormatter())
        {
            DialogCancelationToken = cancelationToken,
            PropertySkipToken = skipToken,
            IsRecursive = true,
        };
        reader.ReadInDialog();
        return reader.IsDialogCanceled;
    }

    public static bool StartCancelableDialog<T>(T instance, string cancelationToken = "c") where T : class
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        var reader = new InstancePropertiesReader(instance, ": ", new PropertyNamesToRegularCaseFormatter())
        {
            DialogCancelationToken = cancelationToken,
            IsRecursive = true,
        };
        reader.ReadInDialog();
        return reader.IsDialogCanceled;
    }
}
