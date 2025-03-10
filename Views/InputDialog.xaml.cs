using System.Windows.Controls;

namespace CPO2.Views;

public partial class InputDialog : Window
{
    public string InputText { get; private set; } = string.Empty;

    public InputDialog(string prompt, string defaultText = "")
    {
        InitializeComponent();
        PromptText.Text = prompt;
        InputBox.Text = defaultText;
        InputBox.Focus();
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        InputText = InputBox.Text;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
