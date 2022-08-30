using System.Text.RegularExpressions;

namespace DevastedSystematics.Console;

public class PropertyNamesToRegularCaseFormatter : IPropertyNameFormatter
{
    private const string notFirstSingleCapitalLetterRegex = @"(?<!^|\p{Lu})\p{Lu}{1}";

    public string Format(string name) =>
        Regex.Replace(name, notFirstSingleCapitalLetterRegex, ChangeCapitalLetterToSpacedLowerLetter);

    private string ChangeCapitalLetterToSpacedLowerLetter(Match match) =>
        $" {match.Value.ToLower()}";
}
