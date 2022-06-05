using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericsPractice
{
    public interface IDataSerializer<out T> where T : class, new()
    {
        T Deserialize();

        void Serialize();
    }

    public class XmlDataSerializer<T> : IDataSerializer<T> where T: class, new()
    {
        public T Deserialize()
        {
            return new T();
        }

        public void Serialize()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRolledSection
    {
        string Designation { get; set; }

        string StaadName { get; set; }
    }

    public class Table<T> where T : class, new()
    {
        public string Name { get; set; }

        public List<T> Profiles { get; set; }
    }


    public class SectionTables<T> where T : class, new()
    {
        public List<Table<T>> Tables { get; set; }
    }

    public class RolledSection : IRolledSection
    {

        public string Designation { get; set; }

        public string StaadName { get; set; }
    }

    public class RolledISection : RolledSection
    {
        public double H { get; set; }

        public double B { get; set; }

        public double Tf { get; set; }

        public double Tw { get; set; }
    }

    public class RolledChsSection : RolledSection
    {
        public double D { get; set; }

        public double Tw { get; set; }
    }

    public class RolledLSection : RolledSection
    {
        public double L { get; set; }

        public double Tw { get; set; }
    }

    public class RolledBoxSection : RolledSection
    {
        public double H { get; set; }

        public double B { get; set; }

        public double Tw { get; set; }
    }
}
