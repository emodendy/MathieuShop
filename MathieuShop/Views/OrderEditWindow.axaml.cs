using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MathieuShop.Data;
using System.Globalization;

namespace MathieuShop.Views
{
    public partial class OrderEditWindow : Window
    {
        private readonly int? orderId;

        private static readonly string[] statuses =
        {
            "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
        };

        public OrderEditWindow() : this(null) { }

        public OrderEditWindow(int? orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            LoadCombos();

            if (orderId.HasValue)
            {
                WindowTitle.Text = "Редактировать заказ";
                LoadOrder(orderId.Value);
            }
            else
            {
                WindowTitle.Text = "Добавить заказ";
                OrderDatePicker.SelectedDate = DateTime.UtcNow;
                StatusCombo.SelectedIndex = 0;
            }
        }

        private void LoadCombos()
        {
            var users = App.DbContext.Users.OrderBy(u => u.Login).ToList();
            UserCombo.ItemsSource = users;
            UserCombo.DisplayMemberBinding = new Avalonia.Data.Binding("Login");

            StatusCombo.ItemsSource = statuses;
        }

        private void LoadOrder(int id)
        {
            var order = App.DbContext.Orders.Find(id);
            if (order == null) return;

            var users = UserCombo.ItemsSource as System.Collections.Generic.List<User>;
            UserCombo.SelectedItem = users?.FirstOrDefault(u => u.UserId == order.UserId);

            // Normalize to UTC for display
            OrderDatePicker.SelectedDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
            StatusCombo.SelectedItem = order.Status;
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (UserCombo.SelectedItem is not User user)
            { ShowError("Выберите пользователя."); return; }

            if (OrderDatePicker.SelectedDate == null)
            { ShowError("Выберите дату заказа."); return; }

            if (StatusCombo.SelectedItem is not string status)
            { ShowError("Выберите статус."); return; }

            // Ensure UTC kind for PostgreSQL compatibility
            DateTime selectedDate = DateTime.SpecifyKind(
                OrderDatePicker.SelectedDate ?? DateTime.UtcNow,
                DateTimeKind.Utc);

            if (orderId.HasValue)
            {
                var entity = App.DbContext.Orders.Find(orderId.Value);
                if (entity != null)
                {
                    entity.UserId = user.UserId;
                    entity.OrderDate = selectedDate;
                    entity.Status = status;
                    entity.UpdatedAt = DateTime.UtcNow;
                    App.DbContext.SaveChanges();
                }
            }
            else
            {
                App.DbContext.Orders.Add(new Order
                {
                    UserId = user.UserId,
                    OrderDate = selectedDate,
                    Status = status,
                    UpdatedAt = DateTime.UtcNow
                });
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
