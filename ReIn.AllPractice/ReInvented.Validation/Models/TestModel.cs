using ReInvented.Shared.Base;
using ReInvented.Shared.Stores;

namespace ReInvented.Validation.Models
{
    public abstract class ValidationPropertyStore : PropertyStore
    {
        public NotifyDataErrorInfo NotifyDataErrorInfo { get; set; }


    }

    public class TestModel : ValidationPropertyStore
    {
        override 
    }
}
