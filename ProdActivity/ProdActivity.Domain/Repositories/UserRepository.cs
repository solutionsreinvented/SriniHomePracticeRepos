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

        public IUser GetById(int id)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IUser GetByEmployeeId(string employeeId)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
            return context.Users.FirstOrDefault(u => u.EmployeeId == employeeId);
        }

        public List<IUser> GetAllUsers()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            var users = context.Users.ToList();

            if (users == null || users.Count == 0)
            {
                var adminUser = new User 
                { 
                    EmployeeId = "admin", 
                    FullName = "admin", 
                    Password = "admin", 
                    UserRole = ProdActivity.Domain.Enums.UserRole.Admin 
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
                users.Add(adminUser);
            }

            return users.Cast<IUser>().ToList();
        }

        public void SaveUsers(List<IUser> users)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

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
            
            // Need to reset IDs when re-inserting, or let SQLite generate them.
            // Since we might rely on specific IDs, we'll insert them as is.
            context.Users.AddRange(domainUsers);
            context.SaveChanges();
        }
    }
}
