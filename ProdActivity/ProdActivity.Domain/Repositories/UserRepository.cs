using System.Collections.Generic;
using System.Linq;

using ProdActivity.Domain.Base;
using ProdActivity.Domain.Data;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Repositories
{
    public class UserRepository
    {
        public UserRepository()
        {
        }

        private void EnsureSeeded(AppDbContext context)
        {
            context.Database.EnsureCreated();
            
            if (!context.Users.Any())
            {
                var adminUser = new User 
                { 
                    EmployeeId = "admin", 
                    FullName = "System Admin", 
                    Password = "admin", 
                    UserRole = ProdActivity.Domain.Enums.UserRole.Admin 
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }

        public IUser GetById(int id)
        {
            using var context = new AppDbContext();
            EnsureSeeded(context);
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IUser GetByEmployeeId(string employeeId)
        {
            using var context = new AppDbContext();
            EnsureSeeded(context);
            return context.Users.FirstOrDefault(u => u.EmployeeId == employeeId);
        }

        public List<IUser> GetAllUsers()
        {
            using var context = new AppDbContext();
            EnsureSeeded(context);
            return context.Users.Cast<IUser>().ToList();
        }

        public void SaveUsers(List<IUser> users)
        {
            using var context = new AppDbContext();
            EnsureSeeded(context);

            // Clear existing users and replace
            context.Users.RemoveRange(context.Users);

            var domainUsers = users.Select(u => new User 
            { 
                Id = u.Id, 
                EmployeeId = u.EmployeeId, 
                FullName = u.FullName, 
                Password = u.Password, 
                UserRole = u.UserRole 
            }).ToList();
            
            context.Users.AddRange(domainUsers);
            context.SaveChanges();
        }
    }
}
