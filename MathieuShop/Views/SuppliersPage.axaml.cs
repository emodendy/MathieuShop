using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MathieuShop.Data;

namespace MathieuShop.Views
{
    public partial class SuppliersPage : UserControl
    {
        private List<Supplier> allSuppliers = new();
        private int currentPage = 1;
        private const int pageSize = 5;
        private string sortColumn = "Name";
        private bool sortAscending = true;

        public SuppliersPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            allSuppliers = App.DbContext.Suppliers.ToList();
            ApplySortAndPage();
        }

        private void ApplySortAndPage()
        {
            IEnumerable<Supplier> sorted = sortColumn switch
            {
                "Contact" => sortAscending
                    ? allSuppliers.OrderBy(s => s.ContactName)
                    : allSuppliers.OrderByDescending(s => s.ContactName),
                _ => sortAscending
                    ? allSuppliers.OrderBy(s => s.CompanyName)
                    : allSuppliers.OrderByDescending(s => s.CompanyName),
            };

            int totalPages = Math.Max(1, (int)Math.Ceiling(sorted.Count() / (double)pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            var pageData = sorted
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            SuppliersGrid.ItemsSource = pageData;
            PageInfoText.Text = $"Страница {currentPage} из {totalPages}";
            TotalRecordsText.Text = $"Всего: {allSuppliers.Count}";
        }

        private void SortNameButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Name") sortAscending = !sortAscending;
            else { sortColumn = "Name"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void SortContactButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sortColumn == "Contact") sortAscending = !sortAscending;
            else { sortColumn = "Contact"; sortAscending = true; }
            currentPage = 1; ApplySortAndPage();
        }

        private void PrevPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (currentPage > 1) { currentPage--; ApplySortAndPage(); }
        }

        private void NextPageButton_Click(object? sender, RoutedEventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(allSuppliers.Count / (double)pageSize));
            if (currentPage < totalPages) { currentPage++; ApplySortAndPage(); }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var window = new SupplierEditWindow(null);
            var parent = this.VisualRoot as Window;
            await window.ShowDialog(parent!);
            LoadData();
        }

        private async void EditSupplierButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Supplier supplier)
            {
                var window = new SupplierEditWindow(supplier.SupplierId);
                var parent = this.VisualRoot as Window;
                await window.ShowDialog(parent!);
                LoadData();
            }
        }

        private async void DeleteSupplierButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Supplier supplier)
            {
                var dialog = new ConfirmDialog($"Удалить поставщика \"{supplier.CompanyName}\"?");
                var parent = this.VisualRoot as Window;
                var result = await dialog.ShowDialog<bool>(parent!);
                if (result)
                {
                    var entity = App.DbContext.Suppliers.Find(supplier.SupplierId);
                    if (entity != null)
                    {
                        App.DbContext.Suppliers.Remove(entity);
                        App.DbContext.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}
