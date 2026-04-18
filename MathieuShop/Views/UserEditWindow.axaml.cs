using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MathieuShop.Data;

namespace MathieuShop.Views
{
    public partial class UserEditWindow : Window
    {
        private readonly int? userId;

        public UserEditWindow() : this(null) { }

        public UserEditWindow(int? userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadRoles();

            if (userId.HasValue)
            {
                WindowTitle.Text = "Редактировать пользователя";
                LoadUser(userId.Value);
            }
            else
            {
                WindowTitle.Text = "Добавить пользователя";
            }
        }

        private void LoadRoles()
        {
            var roles = App.DbContext.Roles.OrderBy(r => r.Name).ToList();
            RoleCombo.ItemsSource = roles;
            RoleCombo.DisplayMemberBinding = new Avalonia.Data.Binding("Name");
        }

        private void LoadUser(int id)
        {
            var user = App.DbContext.Users.Find(id);
            if (user == null) return;

            LoginBox.Text = user.Login;
            PasswordBox.Text = user.Password;
            FullNameBox.Text = user.FullName;
            EmailBox.Text = user.Email;

            var roles = RoleCombo.ItemsSource as System.Collections.Generic.List<Role>;
            RoleCombo.SelectedItem = roles?.FirstOrDefault(r => r.RoleId == user.RoleId);
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text))
            { ShowError("Введите логин."); return; }

            if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            { ShowError("Введите пароль."); return; }

            if (RoleCombo.SelectedItem is not Role role)
            { ShowError("Выберите роль."); return; }

            // Check login uniqueness
            var existingLogin = App.DbContext.Users
                .FirstOrDefault(u => u.Login == LoginBox.Text.Trim() && u.UserId != (userId ?? 0));
            if (existingLogin != null)
            { ShowError("Пользователь с таким логином уже существует."); return; }

            if (userId.HasValue)
            {
                var entity = App.DbContext.Users.Find(userId.Value);
                if (entity != null)
                {
                    entity.Login = LoginBox.Text.Trim();
                    entity.Password = PasswordBox.Text;
                    entity.FullName = FullNameBox.Text?.Trim();
                    entity.Email = EmailBox.Text?.Trim();
                    entity.RoleId = role.RoleId;
                    entity.UpdatedAt = DateTime.UtcNow;
                    App.DbContext.SaveChanges();
                }
            }
            else
            {
                var newUser = new User
                {
                    Login = LoginBox.Text.Trim(),
                    Password = PasswordBox.Text,
                    FullName = FullNameBox.Text?.Trim(),
                    Email = EmailBox.Text?.Trim(),
                    RoleId = role.RoleId,
                    UpdatedAt = DateTime.UtcNow
                };
                App.DbContext.Users.Add(newUser);
                App.DbContext.SaveChanges();
            }

            Close();
        }

        private void ShowError(string msg)
        {
            ErrorText.Text = msg;
            ErrorText.IsVisible = true;
        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
            => Close();
    }
}
