using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MathieuShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MathieuShop.Views
{
    public partial class OrdersPage : UserControl
    {
        private List<Order> allOrders = new();
        private int currentPage = 1;
        private const int pageSize = 5;
        private string sortColumn = "Date";
        private bool sortAscending = false;

        public OrdersPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            allOrders = App.DbContext.Orders
                .Include(o => o.User)
                .ToList();
            ApplySortAndPage();
        }

        private void ApplySortAndPage()
        {
            IEnumerable<Order> sorted = sortColumn switch
            {
                "Status" => sortAscending
                    ? allOrders.OrderBy(o => o.Status)
                    : allOrders.OrderByDescending(o => o.Status),
                "User" => sortAscending
                    ? allOrders.OrderBy(o => o.User.Login)
                    : allOrders.OrderByDescending(o => o.User.Login),
                _ => sortAscending
                    ? allOrders.OrderBy(o => o.OrderDate)
                    : allOrders.OrderByDescending(o => o.OrderDate),
            };

            int totalPages = Math.Max(1, (int)Math.Ceiling(sorted.Count() / (double)pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            var pageData = sorted
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            OrdersGrid.ItemsSource = pageData;
            PageInfoText.Text = $"Страница {currentPage} из {totalPages}";
            TotalRecordsText.Text = $"Всего: {allOrders.Count}";
        }

        private void SortDateButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Date") sortAscending = !sortAscending;
            else { sortColumn = "Date"; sortAscending = false; }
            currentPage = 1; ApplySortAndPage();
        }

        private void SortStatusButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Status") sortAscending = !sortAscending;
            else { sortColumn = "Status"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void SortUserButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "User") sortAscending = !sortAscending;
            else { sortColumn = "User"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void PrevPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (currentPage > 1) { currentPage--; ApplySortAndPage(); }
        }

        private void NextPageButton_Click(object? sender, RoutedEventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(allOrders.Count / (double)pageSize));
            if (currentPage < totalPages) { currentPage++; ApplySortAndPage(); }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var window = new OrderEditWindow(null);
            var parent = this.VisualRoot as Window;
            await window.ShowDialog(parent!);
            LoadData();
        }

        private async void EditOrderButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Order order)
            {
                var window = new OrderEditWindow(order.OrderId);
                var parent = this.VisualRoot as Window;
                await window.ShowDialog(parent!);
                LoadData();
            }
        }

        private async void DeleteOrderButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Order order)
            {
                var dialog = new ConfirmDialog($"Удалить заказ #{order.OrderId}?");
                var parent = this.VisualRoot as Window;
                var result = await dialog.ShowDialog<bool>(parent!);
                if (result)
                {
                    var entity = App.DbContext.Orders.Find(order.OrderId);
                    if (entity != null)
                    {
                        App.DbContext.Orders.Remove(entity);
                        App.DbContext.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}
