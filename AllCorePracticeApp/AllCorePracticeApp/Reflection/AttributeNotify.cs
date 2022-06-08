using ReInvented.Shared.Stores;

namespace AllCorePracticeApp.Reflection
{
    public class AttributeNotify : PropertyStore
    {
        public AttributeNotify()
        {

        }




        public int Id { get => Get<int>(); set => Set(value); }

        public string First { get; set; }

        public string Last { get; set; }

        public string Full => $"{First} {Last}";
    }
}
