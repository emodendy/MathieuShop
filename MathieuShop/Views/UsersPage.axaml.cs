using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MathieuShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MathieuShop.Views
{
    public partial class UsersPage : UserControl
    {
        private List<User> allUsers = new();
        private int currentPage = 1;
        private const int pageSize = 5;
        private string sortColumn = "Login";
        private bool sortAscending = true;

        public UsersPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            allUsers = App.DbContext.Users
                .Include(u => u.Role)
                .ToList();
            ApplySortAndPage();
        }

        private void ApplySortAndPage()
        {
            IEnumerable<User> sorted = sortColumn switch
            {
                "Role" => sortAscending
                    ? allUsers.OrderBy(u => u.Role.Name)
                    : allUsers.OrderByDescending(u => u.Role.Name),
                "Name" => sortAscending
                    ? allUsers.OrderBy(u => u.FullName)
                    : allUsers.OrderByDescending(u => u.FullName),
                _ => sortAscending
                    ? allUsers.OrderBy(u => u.Login)
                    : allUsers.OrderByDescending(u => u.Login),
            };

            int totalPages = Math.Max(1, (int)Math.Ceiling(sorted.Count() / (double)pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            var pageData = sorted
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            UsersGrid.ItemsSource = pageData;
            PageInfoText.Text = $"Страница {currentPage} из {totalPages}";
            TotalRecordsText.Text = $"Всего: {allUsers.Count}";
        }

        private void SortLoginButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Login") sortAscending = !sortAscending;
            else { sortColumn = "Login"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void SortRoleButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Role") sortAscending = !sortAscending;
            else { sortColumn = "Role"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void SortNameButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Name") sortAscending = !sortAscending;
            else { sortColumn = "Name"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void PrevPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (currentPage > 1) { currentPage--; ApplySortAndPage(); }
        }

        private void NextPageButton_Click(object? sender, RoutedEventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(allUsers.Count / (double)pageSize));
            if (currentPage < totalPages) { currentPage++; ApplySortAndPage(); }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var window = new UserEditWindow(null);
            var parent = this.VisualRoot as Window;
            await window.ShowDialog(parent!);
            LoadData();
        }

        private async void EditUserButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is User user)
            {
                var window = new UserEditWindow(user.UserId);
                var parent = this.VisualRoot as Window;
                await window.ShowDialog(parent!);
                LoadData();
            }
        }

        private async void DeleteUserButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is User user)
            {
                if (user.UserId == CurrentSession.CurrentUser?.UserId)
                {
                    var info = new ConfirmDialog("Нельзя удалить текущего пользователя.");
                    var parent = this.VisualRoot as Window;
                    await info.ShowDialog<bool>(parent!);
                    return;
                }

                var dialog = new ConfirmDialog($"Удалить пользователя \"{user.Login}\"?");
                var parent2 = this.VisualRoot as Window;
                var result = await dialog.ShowDialog<bool>(parent2!);
                if (result)
                {
                    var entity = App.DbContext.Users.Find(user.UserId);
                    if (entity != null)
                    {
                        App.DbContext.Users.Remove(entity);
                        App.DbContext.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}
