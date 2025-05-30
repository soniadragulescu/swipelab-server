namespace SwipeLab.Domain.DatingProfile.Utils
{
    public static class EnumUtils
    {
        public static string FormatEnumValue(Enum value)
        {
            return value.ToString()
                        .Replace("_", " ")
                        .ToLowerInvariant();
        }
    }

}
