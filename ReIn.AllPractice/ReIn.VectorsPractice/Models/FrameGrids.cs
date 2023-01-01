using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReIn.VectorsPractice.Models
{
    public class FrameGrids
    {
        public int Id { get; set; }

        public string CalculatedId => Id.ToString();

        public int Calculate => CalculatedId.ToString();
    }
}
