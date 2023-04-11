using System.IO;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

namespace ActivityTracker.Domain.Services
{
    public static class ProjectMasterService
    {
        private static readonly IDataSerializer<ProjectMaster> _serializer = new JsonDataSerializer<ProjectMaster>();

        public static ProjectMaster Retrieve()
        {
            /// TODO: In a real-world scenario this needs to generated from a json file.

            ProjectMaster projectMaster = new();

            IProject preOrder1 = new PreOrder("GS2212129", "Liberia");
            IProject preOrder2 = new PreOrder("GS2212130", "Rilebia");
            IProject preOrder3 = new PreOrder("GS2212131", "Bileria");

            IProject order1 = new Order("22-3849", "25m Thickener");
            IProject order2 = new Order("22-3877", "48m Thickener");
            IProject order3 = new Order("22-3965", "77m Thickener");

            projectMaster.Projects.Add(preOrder1);
            projectMaster.Projects.Add(preOrder2);
            projectMaster.Projects.Add(preOrder3);
            projectMaster.Projects.Add(order1);
            projectMaster.Projects.Add(order2);
            projectMaster.Projects.Add(order3);

            return projectMaster;
        }

        public static ProjectMaster ReadFromFile(string fileFullPath = null)
        {
            string filePath = fileFullPath ?? FileServiceProvider.ProjectMasterFilePath;

            return _serializer.Deserialize(filePath);
        }

        public static void SaveToFile(ProjectMaster projectMaster, string fileFullPath = null)
        {
            string filePath = fileFullPath ?? FileServiceProvider.ProjectMasterFilePath;

            string serializedContents =  _serializer.Serialize(projectMaster);

            File.WriteAllText(filePath, serializedContents);
        }
    }
}
