namespace ProductsSample.Abstractions.Extensions;

public static class EnumExtensions
{
    public static List<string> EnumFlagsToList(Enum flagsEnum)
    {
        return Enum.GetValues(flagsEnum.GetType())
            .Cast<Enum>()
            .Where(value => flagsEnum.HasFlag(value) && Convert.ToInt32(value) != 0)
            .Select(value => value.ToString())
            .ToList();
    }
}