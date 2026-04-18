using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MathieuShop.Data;

namespace MathieuShop.Views
{
    public partial class CategoriesPage : UserControl
    {
        private List<Category> allCategories = new();
        private int currentPage = 1;
        private const int pageSize = 5;
        private bool sortAscending = true;

        public CategoriesPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            allCategories = App.DbContext.Categories.ToList();
            ApplySortAndPage();
        }

        private void ApplySortAndPage()
        {
            IEnumerable<Category> sorted = sortAscending
                ? allCategories.OrderBy(c => c.Name)
                : allCategories.OrderByDescending(c => c.Name);

            int totalPages = Math.Max(1, (int)Math.Ceiling(sorted.Count() / (double)pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            var pageData = sorted
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            CategoriesGrid.ItemsSource = pageData;
            PageInfoText.Text = $"Страница {currentPage} из {totalPages}";
            TotalRecordsText.Text = $"Всего: {allCategories.Count}";
        }

        private void SortNameButton_Click(object? sender, RoutedEventArgs e)
        {
            sortAscending = !sortAscending;
            currentPage = 1;
            ApplySortAndPage();
        }

        private void PrevPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (currentPage > 1) { currentPage--; ApplySortAndPage(); }
        }

        private void NextPageButton_Click(object? sender, RoutedEventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(allCategories.Count / (double)pageSize));
            if (currentPage < totalPages) { currentPage++; ApplySortAndPage(); }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var window = new CategoryEditWindow(null);
            var parent = this.VisualRoot as Window;
            await window.ShowDialog(parent!);
            LoadData();
        }

        private async void EditCategoryButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Category category)
            {
                var window = new CategoryEditWindow(category.CategoryId);
                var parent = this.VisualRoot as Window;
                await window.ShowDialog(parent!);
                LoadData();
            }
        }

        private async void DeleteCategoryButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Category category)
            {
                var dialog = new ConfirmDialog($"Удалить категорию \"{category.Name}\"?");
                var parent = this.VisualRoot as Window;
                var result = await dialog.ShowDialog<bool>(parent!);
                if (result)
                {
                    var entity = App.DbContext.Categories.Find(category.CategoryId);
                    if (entity != null)
                    {
                        App.DbContext.Categories.Remove(entity);
                        App.DbContext.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}
