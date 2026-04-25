using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProdActivity.Domain.Data;
using ProdActivity.Domain.Data.Entities;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Models;

namespace ProdActivity.Domain.Repositories
{
    public class DbRepository
    {
        private readonly string _dbPath = "Data Source=ProdActivity.db";

        public void InitializeDatabase()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
        }

        public void SaveProjectMaster(ProjectMaster projectMaster)
        {
            using var context = new AppDbContext();
            
            // For simplicity in this full-fledged transition, we recreate the graph
            // In a production app, we would update existing entities.
            // Since this is replacing a file overwrite, wiping and rewriting is identical behavior
            // to the original JSON file overwrite.
            
            context.Database.ExecuteSqlRaw("DELETE FROM ActivityResources");
            context.Database.ExecuteSqlRaw("DELETE FROM Activities");
            context.Database.ExecuteSqlRaw("DELETE FROM Projects");
            
            var usedIds = new HashSet<string>();

            foreach (var p in projectMaster.Projects)
            {
                var dbProject = new DbProject
                {
                    Code = p.Code,
                    Name = p.Name,
                    Type = p.Type
                };

                foreach (var a in p.Activities)
                {
                    string activityId = a.Id;
                    if (string.IsNullOrWhiteSpace(activityId) || usedIds.Contains(activityId))
                    {
                        activityId = System.Guid.NewGuid().ToString();
                    }
                    usedIds.Add(activityId);

                    var dbActivity = new DbActivity
                    {
                        Id = activityId,
                        Discipline = a.Discipline,
                        CategoryName = a.Category?.Name,
                        SubCategory = a.SubCategory,
                        Description = a.Description,
                        InitiatedOn = a.InitiatedOn,
                        AllocatedHours = a.AllocatedHours,
                        CurrentStatus = a.CurrentStatus,
                        ScheduledCompletion = a.ScheduledCompletion
                    };
                    
                    dbProject.Activities.Add(dbActivity);
                }
                context.Projects.Add(dbProject);
            }
            context.SaveChanges();
        }

        public ProjectMaster LoadProjectMaster()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            var projectMaster = new ProjectMaster();
            
            var dbProjects = context.Projects.Include(p => p.Activities).ToList();

            foreach (var dbp in dbProjects)
            {
                IProject project;
                if (dbp.Type == ProjectType.Order) project = new Order(dbp.Code, dbp.Name);
                else if (dbp.Type == ProjectType.Development) project = new Development(dbp.Code, dbp.Name);
                else project = new PreOrder(dbp.Code, dbp.Name);

                foreach (var dba in dbp.Activities)
                {
                    IActivity activity;
                    if (dba.Discipline == Discipline.Design) activity = new DesignActivity(project);
                    else if (dba.Discipline == Discipline.Detailing) activity = new DetailingActivity(project);
                    else activity = new DevelopmentActivity(project);

                    // activity.Category = new Category { Name = dba.CategoryName }; // Needs ActivityMaster to resolve
                    activity.SubCategory = dba.SubCategory;
                    activity.Description = dba.Description;
                    activity.InitiatedOn = dba.InitiatedOn;
                    activity.AllocatedHours = dba.AllocatedHours;
                    activity.CurrentStatus = dba.CurrentStatus;

                    project.Activities.Add(activity);
                }

                projectMaster.Projects.Add(project);
            }

            return projectMaster;
        }
    }
}
