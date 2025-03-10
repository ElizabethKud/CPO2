using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

public partial class MainWindow : Window
{
    private AppDbContext _db = new();
    private User _currentUser;

    public MainWindow()
    {
        InitializeComponent();
        ShowLoginWindow();
        LoadTree();

        if (_currentUser.Role == "admin")
        {
            AdminButton.Visibility = Visibility.Visible;
        }
    }

    private void OpenAdminWindow_Click(object sender, RoutedEventArgs e)
    {
        new UsersAdminWindow(_currentUser).ShowDialog();
    }


    private void ShowLoginWindow()
    {
        var loginWindow = new LoginWindow();
        if (loginWindow.ShowDialog() == true)
        {
            _currentUser = loginWindow.LoggedInUser;
            MessageBox.Show($"Добро пожаловать, {_currentUser.Username}!", "Вход выполнен", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            Close();
        }
    }

    private void LoadTree()
    {
        TreeViewBooks.Items.Clear();
        var genres = _db.Genres.Include(g => g.Series).ThenInclude(s => s.Books).ToList();
        foreach (var genre in genres)
            TreeViewBooks.Items.Add(genre);
    }

    private void AddGenre_Click(object sender, RoutedEventArgs e)
    {
        var input = new InputDialog("Введите название жанра:");
        if (input.ShowDialog() == true)
        {
            _db.Genres.Add(new Genre { GenreName = input.InputText });
            _db.SaveChanges();
            LoadTree();
        }
    }

    private void AddSeries_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Genre genre) return;
        var input = new InputDialog("Введите название серии:");
        if (input.ShowDialog() == true)
        {
            _db.Series.Add(new Series { SeriesName = input.InputText, GenreId = genre.Id });
            _db.SaveChanges();
            LoadTree();
        }
    }

    private void AddBook_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Series series) return;
        var input = new InputDialog("Введите название книги:");
        if (input.ShowDialog() == true)
        {
            _db.Books.Add(new Book { Title = input.InputText, SeriesId = series.Id });
            _db.SaveChanges();
            LoadTree();
        }
    }
    
        // Редактирование жанра
    private void EditGenre_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Genre genre) return;
        var input = new InputDialog("Введите новое название жанра:", genre.GenreName);
        if (input.ShowDialog() == true)
        {
            genre.GenreName = input.InputText;
            _db.SaveChanges();
            LoadTree();
        }
    }

    // Удаление жанра
    private void DeleteGenre_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Genre genre) return;
        if (MessageBox.Show("Удалить жанр? Это удалит все серии и книги!", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _db.Genres.Remove(genre);
            _db.SaveChanges();
            LoadTree();
        }
    }

    // Редактирование серии
    private void EditSeries_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Series series) return;
        var input = new InputDialog("Введите новое название серии:", series.SeriesName);
        if (input.ShowDialog() == true)
        {
            series.SeriesName = input.InputText;
            _db.SaveChanges();
            LoadTree();
        }
    }

    // Удаление серии
    private void DeleteSeries_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Series series) return;
        if (MessageBox.Show("Удалить серию? Это удалит все книги!", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _db.Series.Remove(series);
            _db.SaveChanges();
            LoadTree();
        }
    }

    // Редактирование книги
    private void EditBook_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Book book) return;
        var input = new InputDialog("Введите новое название книги:", book.Title);
        if (input.ShowDialog() == true)
        {
            book.Title = input.InputText;
            _db.SaveChanges();
            LoadTree();
        }
    }

    // Удаление книги
    private void DeleteBook_Click(object sender, RoutedEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not Book book) return;
        if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _db.Books.Remove(book);
            _db.SaveChanges();
            LoadTree();
        }
    }

    private object _draggedItem; // Храним перемещаемый объект

// Захват объекта при начале перетаскивания
    private void TreeViewBooks_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (TreeViewBooks.SelectedItem is not null)
        {
            _draggedItem = TreeViewBooks.SelectedItem;
        }
    }

// Начало перемещения
    private void TreeViewBooks_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && _draggedItem != null)
        {
            DragDrop.DoDragDrop(TreeViewBooks, _draggedItem, DragDropEffects.Move);
        }
    }

// Обработчик сброса элемента
    private void TreeViewBooks_Drop(object sender, DragEventArgs e)
    {
        if (_draggedItem is Series series && e.OriginalSource is FrameworkElement element)
        {
            if (element.DataContext is Genre newGenre && series.GenreId != newGenre.Id)
            {
                series.GenreId = newGenre.Id;
                _db.SaveChanges();
                LoadTree();
            }
        }
        else if (_draggedItem is Book book && e.OriginalSource is FrameworkElement elementBook)
        {
            if (elementBook.DataContext is Series newSeries && book.SeriesId != newSeries.Id)
            {
                book.SeriesId = newSeries.Id;
                _db.SaveChanges();
                LoadTree();
            }
        }
        _draggedItem = null; // Сброс перемещаемого объекта
    }

}
