using System.IO;

using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Models;
using ProdActivity.Domain.Repositories;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

namespace ProdActivity.Domain.Services
{
    public static class ProjectMasterService
    {
        private static readonly IDataSerializer<ProjectMaster> _serializer = new JsonDataSerializer<ProjectMaster>();
        private static readonly DbRepository _dbRepository = new DbRepository();

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
            // Transitioning to DB Load
            var projectMaster = _dbRepository.LoadProjectMaster();
            
            // Fallback to JSON if DB is empty
            if (projectMaster.Projects.Count == 0 && File.Exists(FileServiceProvider.ProjectMasterFilePath))
            {
                string filePath = fileFullPath ?? FileServiceProvider.ProjectMasterFilePath;
                var jsonMaster = _serializer.DeserializeText(File.ReadAllText(filePath));
                
                // Migrate to DB
                if (jsonMaster != null && jsonMaster.Projects.Count > 0)
                {
                    _dbRepository.SaveProjectMaster(jsonMaster);
                    return jsonMaster;
                }
            }

            return projectMaster;
        }

        public static void SaveToFile(ProjectMaster projectMaster, string fileFullPath = null)
        {
            // Save to SQLite DB
            _dbRepository.SaveProjectMaster(projectMaster);

            // Keep saving to JSON for backup
            string filePath = fileFullPath ?? FileServiceProvider.ProjectMasterFilePath;

            if (!Directory.Exists(FileServiceProvider.BackupDirectory))
            {
                Directory.CreateDirectory(FileServiceProvider.BackupDirectory);
            }
            if (File.Exists(filePath))
            {
                 File.Copy(filePath, FileServiceProvider.BackupFilePath, true);
            }

            string serializedContents =  _serializer.Serialize(projectMaster);
            File.WriteAllText(filePath, serializedContents);
        }
    }
}

