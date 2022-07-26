using System.Globalization;


foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
{
    if (culture.Name.StartsWith("en"))
    {
        string decimalSeparator = culture.NumberFormat.NumberDecimalSeparator == "," ? "comma" : "dot";
        Console.WriteLine($"{culture.NativeName} ({culture.Name}) {culture.DateTimeFormat.ShortDatePattern} {decimalSeparator}");
    }
}