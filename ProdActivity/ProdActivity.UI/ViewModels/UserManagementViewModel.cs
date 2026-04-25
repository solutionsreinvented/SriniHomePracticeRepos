using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Repositories;
using ProdActivity.UI.Base;
using ProdActivity.UI.Commands;
using ProdActivity.UI.Stores;

namespace ProdActivity.UI.ViewModels
{
    public class UserManagementViewModel : ManageUserViewModel
    {
        public ObservableCollection<User> Users { get; set; }

        public ICommand AddUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public User SelectedUser
        {
            get => Get<User>();
            set => Set(value);
        }

        public UserManagementViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
        }

        protected override void Initialize()
        {
            LoadUsers();

            AddUserCommand = new RelayCommand(OnAddUser, true);
            DeleteUserCommand = new RelayCommand(OnDeleteUser, true);
            SaveCommand = new RelayCommand(OnSave, true);
        }

        private void LoadUsers()
        {
            // Create a new instance because base class usersRepository is private
            var repo = new UserRepository();
            var users = repo.GetAllUsers().Cast<User>().ToList();
            Users = new ObservableCollection<User>(users);
        }

        private void OnAddUser()
        {
            var addUserViewModel = new AddUserViewModel();
            bool? result = _dialogService.ShowDialog(addUserViewModel);

            if (result == true)
            {
                var newUser = addUserViewModel.NewUser;
                // Generate simple ID if needed (though DB handles it, good for UI sync)
                newUser.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
                Users.Add(newUser);
                SelectedUser = newUser;
            }
        }

        private void OnDeleteUser(object parameter)
        {
            var userToDelete = parameter as User ?? SelectedUser;
            if (userToDelete != null && userToDelete.Id != 1) // Prevent deleting root admin
            {
                Users.Remove(userToDelete);
            }
        }

        private void OnSave()
        {
            var repo = new UserRepository();
            repo.SaveUsers(Users.Cast<IUser>().ToList());
            System.Windows.MessageBox.Show("Users saved successfully.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
