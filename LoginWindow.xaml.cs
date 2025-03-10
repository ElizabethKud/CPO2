using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public partial class LoginWindow : Window
{
    private AppDbContext _db = new();
    public User? LoggedInUser { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var storedHashParts = user.PasswordHash.Split(':');
        if (storedHashParts.Length != 2)
        {
            MessageBox.Show("Ошибка данных пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string salt = storedHashParts[0];
        string storedHash = storedHashParts[1];
        string inputHash = HashPassword(password, salt);

        if (inputHash != storedHash)
        {
            MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        LoggedInUser = user;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = salt + password;
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hash);
        }
    }
}