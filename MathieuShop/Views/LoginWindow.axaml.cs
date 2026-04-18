using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MathieuShop.Data;
using MathieuShop.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MathieuShop.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            var logoPath = ResourceHelper.GetImageUri("Ресурсы/Logo.png");
            if (logoPath != null)
            {
                var bitmap = new Bitmap(logoPath);
                LogoImage.Source = bitmap;
                LogoBigImage.Source = bitmap;
            }
        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog("Выйти из приложения?");
            var result = await dialog.ShowDialog<bool>(this);
            if (result) Close();
        }

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text?.Trim() ?? string.Empty;
            var password = PasswordBox.Text ?? string.Empty;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Заполните все поля.");
                return;
            }

            var user = App.DbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                ShowError("Неверный логин или пароль.");
                return;
            }

            if (user.Role.Name != "Admin")
            {
                ShowError("Доступ разрешён только администратору.");
                return;
            }

            CurrentSession.CurrentUser = user;

            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.IsVisible = true;
        }
    }
}
