using System.Collections.Generic;
using System.IO;
using System.Linq;

using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Mappers;
using ActivityTracker.Domain.Services;

using ReInvented.DataAccess;

namespace ActivityTracker.Domain.Repositories
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
            string fileFullPath = Path.Combine(FileServiceProvider.DataDirectory, _fileName);

            return ClassToInterfaceMapper<User, IUser>.Map(_jsonDataSerializer.Deserialize(fileFullPath));
        }
    }
}
