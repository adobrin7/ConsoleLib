namespace DevastedSystematics.ConsoleLib;

public static class InputPatterns
{
    public const string Name = @"^\p{Lu}{1}\p{Ll}+$";

    public const string FullName = Name + @"\s" + Name;

    public const string FloatingNumber = @"^[0-9]+(\.[0-9]+)?$";

    public const string Phone = @"^(\d[-\s:]?){10}$";
}
