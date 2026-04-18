using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MathieuShop.Data;

namespace MathieuShop.Views
{
    public partial class SupplierEditWindow : Window
    {
        private readonly int? supplierId;

        public SupplierEditWindow() : this(null) { }

        public SupplierEditWindow(int? supplierId)
        {
            InitializeComponent();
            this.supplierId = supplierId;

            if (supplierId.HasValue)
            {
                WindowTitle.Text = "Редактировать поставщика";
                LoadSupplier(supplierId.Value);
            }
            else
            {
                WindowTitle.Text = "Добавить поставщика";
            }
        }

        private void LoadSupplier(int id)
        {
            var entity = App.DbContext.Suppliers.Find(id);
            if (entity == null) return;
            CompanyNameBox.Text = entity.CompanyName;
            ContactNameBox.Text = entity.ContactName;
            PhoneBox.Text = entity.Phone;
            EmailBox.Text = entity.Email;
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CompanyNameBox.Text))
            { ShowError("Введите название компании."); return; }

            if (supplierId.HasValue)
            {
                var entity = App.DbContext.Suppliers.Find(supplierId.Value);
                if (entity != null)
                {
                    entity.CompanyName = CompanyNameBox.Text.Trim();
                    entity.ContactName = ContactNameBox.Text?.Trim();
                    entity.Phone = PhoneBox.Text?.Trim();
                    entity.Email = EmailBox.Text?.Trim();
                    entity.UpdatedAt = DateTime.UtcNow;
                    App.DbContext.SaveChanges();
                }
            }
            else
            {
                App.DbContext.Suppliers.Add(new Supplier
                {
                    CompanyName = CompanyNameBox.Text.Trim(),
                    ContactName = ContactNameBox.Text?.Trim(),
                    Phone = PhoneBox.Text?.Trim(),
                    Email = EmailBox.Text?.Trim(),
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
