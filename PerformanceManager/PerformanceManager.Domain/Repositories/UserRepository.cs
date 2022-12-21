using System.Collections.Generic;
using System.IO;
using System.Linq;

using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Mappers;
using PerformanceManager.Domain.Services;

using SRi.XamlUIThickenerApp.DataAccess;

namespace PerformanceManager.Domain.Repositories
{
    public class UserRepository
    {
        private const string _fileName = @"users.json";

        private readonly JsonDataSerializer<List<User>> _jsonDataSerializer;

        public UserRepository()
        {
            _jsonDataSerializer = new();
        }

        public IUser GetById(int id)
        {
            return GetAllUsers().FirstOrDefault(u => u.Id == id);
        }

        public List<IUser> GetAllUsers()
        {
            string fileFullPath = Path.Combine(FileServiceProvider.GetDataDirectory(), _fileName);

            return ClassToInterfaceMapper<User, IUser>.Map(_jsonDataSerializer.Deserialiaze(fileFullPath));
        }
    }
}
