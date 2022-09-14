using System.Text.RegularExpressions;

namespace DevastedSystematics.ConsoleLib;

public class PropertyNameToRegularCaseFormatter : IPropertyNameFormatter
{
    public PropertyNameToRegularCaseFormatter(string nameSeparator = "") => 
        this.nameSeparator = nameSeparator;    

    private const string notFirstSingleCapitalLetterRegex = @"(?<!^|\p{Lu})\p{Lu}{1}";

    private readonly string nameSeparator;

    public string Format(string name) =>
        Regex.Replace(
            name, 
            notFirstSingleCapitalLetterRegex, 
            ChangeCapitalLetterToSpacedLowerLetter) + 
        nameSeparator;

    private string ChangeCapitalLetterToSpacedLowerLetter(Match match) =>
        $" {match.Value.ToLower()}";
}
