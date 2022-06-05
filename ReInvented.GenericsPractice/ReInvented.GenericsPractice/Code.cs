using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GenericsPractice
{
    public class Code : IPragmatic
    {

        public Code()
        {
            IsPragmatic = true;
        }

        public void CodeClean()
        {
            MessageBox.Show("I am clean!");
        }

        public void OrientToPrinciples()
        {
            MessageBox.Show("I am oriented to principles!");
        }

        public void Ensure()
        {
            MessageBox.Show("Definitely ensured!");
        }

        public bool IsPragmatic { get; set; }
    }
}
