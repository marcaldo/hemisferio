namespace Hemisferio.Client.Shared.Extensions
{
    public static class Text
    {
        public static string Capitalize(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return char.ToUpper(text[0]) + text[1..];
        }
    }
}
