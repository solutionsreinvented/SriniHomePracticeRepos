namespace ReInvented.Domain.Reporting.Extensions
{
    public static class CharExtensions
    {
        public static bool IsDigit(this char value)
        {
            return char.IsDigit(value);
        }
        public static bool IsLetter(this char value)
        {
            return char.IsLetter(value);
        }
    }
}
