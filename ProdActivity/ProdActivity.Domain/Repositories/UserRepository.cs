using System.Collections.Generic;
using System.IO;
using System.Linq;

using ProdActivity.Domain.Base;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Mappers;
using ProdActivity.Domain.Services;

using ReInvented.DataAccess;

namespace ProdActivity.Domain.Repositories
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
