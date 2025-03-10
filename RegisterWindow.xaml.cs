using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public partial class RegisterWindow : Window
{
    private AppDbContext _db = new();

    public RegisterWindow()
    {
        InitializeComponent();
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameBox.Text.Trim();
        string password = PasswordBox.Password.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (_db.Users.Any(u => u.Username == username))
        {
            MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string salt = GenerateSalt();
        string passwordHash = HashPassword(password, salt);

        var user = new User
        {
            Username = username,
            PasswordHash = $"{salt}:{passwordHash}",
            Role = "user",
            RegistrationDate = DateTime.UtcNow,
            IsActive = true
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        MessageBox.Show("Регистрация успешна!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

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
