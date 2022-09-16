namespace SlvParkview.FinanceManager.Extensions
{
    public static class DecimalExtensions
    {
        public static string FormatNumber(this decimal value, string format)
        {
            return value > 0 ? value.ToString(format) : $"({(-1 * value).ToString(format)})";
        }
    }
}
