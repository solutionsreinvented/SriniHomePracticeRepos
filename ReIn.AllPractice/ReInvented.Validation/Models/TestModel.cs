using ReInvented.Shared.Stores;

namespace ReInvented.Validation.Models
{
    public abstract class ValidationPropertyStore : ErrorsEnabledPropertyStore
    {

    }

    public class BaseModel : ErrorsEnabledPropertyStore
    {
        public BaseModel(ErrorsEnabledPropertyStore store)
        {

        }
    }
}
