using System;

using ReInvented.Sections.Domain.Interfaces;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class RolledSectionKey
    {
        public Type KeyType { get; }

        public RolledSectionKey(Type type)
        {
            if (!typeof(IRolledSection).IsAssignableFrom(type))
                throw new ArgumentException("The type must implement IRolledSection.", nameof(type));

            KeyType = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is RolledSectionKey otherKey)
            {
                return KeyType.Equals(otherKey.KeyType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return KeyType.GetHashCode();
        }
    }
}
