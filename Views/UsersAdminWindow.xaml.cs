using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CPO2.Helpers;
using CPO2.Models;
using Microsoft.EntityFrameworkCore;

namespace CPO2.Views // <---- ОБЯЗАТЕЛЬНО должно совпадать с XAML
{
    public partial class UsersAdminWindow : Window
    {
        private readonly AppDbContext _db = new();
        private readonly User _currentUser;

        public ObservableCollection<User> Users { get; set; } = new();

        public ICommand ToggleActiveCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UsersAdminWindow(User currentUser)
        {
            InitializeComponent(); // Должно заработать, если XAML исправлен
            _currentUser = currentUser;

            if (_currentUser.Role != "admin")
            {
                MessageBox.Show("У вас нет прав доступа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
                return;
            }

            DataContext = this;

            ToggleActiveCommand = new RelayCommand<User>(ToggleActive);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);

            LoadUsers();
        }

        private void LoadUsers()
        {
            Users.Clear();
            foreach (var user in _db.Users.AsNoTracking().ToList())
            {
                Users.Add(user);
            }
        }

        private void ToggleActive(User user)
        {
            if (user == null) return;

            user.IsActive = !user.IsActive;
            _db.Users.Update(user);
            _db.SaveChanges();
            LoadUsers();
        }

        private void DeleteUser()
        {
            if (UsersListView.SelectedItem is User user)
            {
                if (user.Id == _currentUser.Id)
                {
                    MessageBox.Show("Нельзя удалить себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Удалить пользователя {user.Username}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
        
