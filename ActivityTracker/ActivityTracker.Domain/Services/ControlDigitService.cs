using System.Linq;

namespace ProdActivity.Domain.Services
{
    public static class ControlDigitService
    {
        public static string Calculate(string description)
        {
            return description.Sum(c => c - '0').ToString();
        }
    }
}
