using System.Collections.Generic;

using ReInvented.StaadPro.Interactivity.Entities;

namespace SPro2023ConsoleApp
{
    public class NodesCollection
    {
        public NodesCollection()
        {
            Nodes = new HashSet<Node>();
        }

        public HashSet<Node> Nodes { get; set; }
    }
}
