using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MathieuShop.Views
{
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog() : this(string.Empty) { }

        public ConfirmDialog(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void YesButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void NoButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
