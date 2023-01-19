using System.Collections.Generic;

namespace StructSkills.Mapping.Models
{
    public class EmployeeRepository
    {
        public static IEnumerable<Employee> GetAll()
        {
            Manager jamalaiah = new Manager() { Id = 1, Name = "Jamalaiah" };
            Manager jamalamma = new Manager() { Id = 2, Name = "Jamalamma" };

            Lead srinivas = new Lead() { Id = 3, Name = "Srinivas", ReportingManager = jamalaiah };
            Lead lakhi = new Lead() { Id = 4, Name = "Lakhi", ReportingManager = jamalamma };

            SeniorEngineer vihan = new SeniorEngineer() { Id = 5, Name = "Vihan", ReportingManager = srinivas };
            JuniorEngineer likhi = new JuniorEngineer() { Id = 6, Name = "Likhi", ReportingManager = lakhi };

            return new List<Employee>() { jamalaiah, jamalamma, srinivas, lakhi, vihan, likhi};
        }
    }
}
