using System.Linq;
using System.Windows;
using System.Windows.Controls;

public partial class UsersAdminWindow : Window
{
    private AppDbContext _db = new();
    private User _currentUser;

    public UsersAdminWindow(User currentUser)
    {
        InitializeComponent();
        _currentUser = currentUser;

        if (_currentUser.Role != "admin")
        {
            MessageBox.Show("У вас нет прав доступа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            Close();
        }
        else
        {
            LoadUsers();
        }
    }

    private void LoadUsers()
    {
        UsersListView.ItemsSource = _db.Users.ToList();
    }

    private void ToggleActive_Click(object sender, RoutedEventArgs e)
    {
        if (UsersListView.SelectedItem is User user)
        {
            user.IsActive = !user.IsActive;
            _db.SaveChanges();
        }
    }

    private void DeleteUser_Click(object sender, RoutedEventArgs e)
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
                _db.Users.Remove(user);
                _db.SaveChanges();
                LoadUsers();
            }
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}