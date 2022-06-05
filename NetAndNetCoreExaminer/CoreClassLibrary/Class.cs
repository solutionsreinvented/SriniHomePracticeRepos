namespace CoreClassLibrary
{
    public class Class
    {
        public Class()
        {

        }

        public double GetFactor(double value)
        {
            var result = value switch
            {
                <= 0.2 => value / 2,
                <= 0.4 => value / 3,
                <= 0.6 => value / 4,
                <= 0.8 => value / 5,
                _ => value / 6,
            };
            return result;
        }

    }
}
