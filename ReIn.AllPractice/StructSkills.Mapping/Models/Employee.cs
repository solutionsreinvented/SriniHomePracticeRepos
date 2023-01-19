namespace StructSkills.Mapping.Models
{
    public enum Designation
    {
        JuniorEngineer,
        SeniorEngineer,
        Lead,
        Manager
    }

    public class Employee
    {
        public Employee()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Designation Designation { get; protected set; }

    }

    public class JuniorEngineer : Employee
    {
        public JuniorEngineer()
        {
            Designation = Designation.JuniorEngineer;
        }

        public Employee ReportingManager { get; set; }

    }

    public class SeniorEngineer : Employee
    {
        public SeniorEngineer()
        {
            Designation = Designation.SeniorEngineer;
        }

        public Employee ReportingManager { get; set; }

    }

    public class Lead : Employee
    {
        public Lead()
        {
            Designation = Designation.Lead;
        }

        public Employee ReportingManager { get; set; }
    }

    public class Manager : Employee
    {
        public Manager()
        {
            Designation = Designation.Manager;
        }
    }
}
