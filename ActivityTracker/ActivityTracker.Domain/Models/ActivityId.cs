
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Services;
using ActivityTracker.Domain.Stores;

using ReInvented.Shared;

namespace ActivityTracker.Domain.Models
{
    public class ActivityId : PropertyStore
    {
        public ActivityId(IProject project, Discipline discipline, Category category, string subCategory, string structure)
        {

        }

        public string Id => GenerateActivityId();

        public IProject Project { get => Get<IProject>(); set => Set(value); }

        public Discipline Discipline { get => Get<Discipline>(); set => Set(value); }

        public Category Category { get => Get<Category>(); set => Set(value); }

        public string SubCategory { get => Get<string>(); set => Set(value); }

        public string Structure { get => Get<string>(); set => Set(value); }

        private string GenerateActivityId()
        {
            if (SubCategory != null && Project != null)
            {
                string projectBasedCode = $"{Project.Type.GetDescription().Substring(0, 3).ToUpper()}-{Project.Code}";
                string activityCode = $"{(int)Discipline}-{ControlDigitService.Calculate(Category.Name)}-{ControlDigitService.Calculate(SubCategory)}-{ControlDigitService.Calculate(Structure)}";

                return $"{projectBasedCode}-{activityCode}";
            }
            return string.Empty;
        }

    }

}
