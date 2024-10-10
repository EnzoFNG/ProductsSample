using System.Text.RegularExpressions;

namespace AuraPass.SDK.Helpers;

public static partial class RegularExpressionsHelper
{
    [GeneratedRegex(@"[^\w\d\s]")]
    public static partial Regex ExceptLettersAndNumbersRegex();

    [GeneratedRegex(@"\p{Cs}")]
    public static partial Regex ExceptEmotesRegex();
}