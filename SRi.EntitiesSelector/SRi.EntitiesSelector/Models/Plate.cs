namespace SRi.EntitiesSelector.Models
{
    public class Plate
    {
        //private static int _currentId;

        //public Plate()
        //{
        //    _currentId += 1;
        //    Id = _currentId;
        //}


        public Plate(int number, Node a, Node b, Node c, Node d)
        {
            Id = number;
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Plate(int number, Node a, Node b, Node c) : this(number, a, b, c, null) { }

        public int Id { get; private set; }

        public Node A { get; set; }

        public Node B { get; set; }

        public Node C { get; set; }

        public Node D { get; set; }

    }
}
