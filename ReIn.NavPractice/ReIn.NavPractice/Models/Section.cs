using ReInvented.Shared.Stores;

namespace ReIn.NavPractice.Models
{
    public class Section : PropertyStore
    {
        public Section()
        {

        }

        public string Designation { get => Get<string>(); set => Set(value); }

        public double Height { get => Get<double>(); set => Set(value); }

        public double Width { get => Get<double>(); set => Set(value); }

        public override string ToString()
        {
            return "Section";
        }
    }
}
