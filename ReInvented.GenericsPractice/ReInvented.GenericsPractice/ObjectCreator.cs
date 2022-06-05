using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericsPractice
{
    public class ObjectCreator<P> : IDisposable where P: class, new()
    {
        public P Create()
        {
            return new P();
        }

        public void Dispose()
        {
            
        }
    }
}
