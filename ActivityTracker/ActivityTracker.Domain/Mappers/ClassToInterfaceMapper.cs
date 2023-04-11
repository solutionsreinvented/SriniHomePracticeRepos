using System.Collections.Generic;
using System.Linq;

namespace ActivityTracker.Domain.Mappers
{
    public static class ClassToInterfaceMapper<TClass, TInterface> where TClass : TInterface
    {
        public static List<TInterface> Map(List<TClass> values)
        {
            IEnumerable<TInterface> result = from v in values select (TInterface)v;

            return result.ToList();
        }

    }
}
