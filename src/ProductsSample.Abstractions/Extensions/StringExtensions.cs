using System.Globalization;
using System.Text;
using static AuraPass.SDK.Helpers.RegularExpressionsHelper;

namespace ProductsSample.Abstractions.Extensions;

public static class StringExtensions
{
    public static bool HasUpperCase(this string text)
        => text.Any(char.IsUpper);

    public static bool HasLowerCase(this string text)
        => text.Any(char.IsLower);

    public static bool HasNumber(this string text)
        => text.Any(char.IsNumber);

    public static bool IsNumeric(this string text)
        => text.Trim().All(char.IsNumber);

    public static bool HasSpecialChar(this string text)
        => text.Any(char.IsSymbol)
        || text.Any(char.IsPunctuation);

    public static bool IsEmpty(this string? text)
        => string.IsNullOrWhiteSpace(text);

    public static string RemoveDiacritics(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string RemoveSpecialCharacters(this string text)
    {
        string cleanedText = ExceptLettersAndNumbersRegex()
            .Replace(text, "");

        cleanedText = ExceptEmotesRegex()
            .Replace(cleanedText, "");

        return cleanedText;
    }
}