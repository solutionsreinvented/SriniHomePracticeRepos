namespace SRi.EntitiesSelector.Models
{
    public class Node
    {
        //private static int _currentId;

        //public Node()
        //{
        //    _currentId += 1;
        //    Id = _currentId;
        //}

        //public Node(double x, double y, double z)// : this()
        //{

        //}

        public Node(int number, double x, double y, double z) //: this(x, y, z)
        {
            Id = number;
            X = x;
            Y = y;
            Z = z;
        }

        public int Id { get; private set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

    }
}
