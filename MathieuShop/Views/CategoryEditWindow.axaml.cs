using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MathieuShop.Data;

namespace MathieuShop.Views
{
    public partial class CategoryEditWindow : Window
    {
        private readonly int? categoryId;

        public CategoryEditWindow() : this(null) { }

        public CategoryEditWindow(int? categoryId)
        {
            InitializeComponent();
            this.categoryId = categoryId;

            if (categoryId.HasValue)
            {
                WindowTitle.Text = "Редактировать категорию";
                LoadCategory(categoryId.Value);
            }
            else
            {
                WindowTitle.Text = "Добавить категорию";
            }
        }

        private void LoadCategory(int id)
        {
            var entity = App.DbContext.Categories.Find(id);
            if (entity == null) return;
            NameBox.Text = entity.Name;
            DescriptionBox.Text = entity.Description;
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            { ShowError("Введите название."); return; }

            if (categoryId.HasValue)
            {
                var entity = App.DbContext.Categories.Find(categoryId.Value);
                if (entity != null)
                {
                    entity.Name = NameBox.Text.Trim();
                    entity.Description = DescriptionBox.Text?.Trim();
                    entity.UpdatedAt = DateTime.UtcNow;
                    App.DbContext.SaveChanges();
                }
            }
            else
            {
                App.DbContext.Categories.Add(new Category
                {
                    Name = NameBox.Text.Trim(),
                    Description = DescriptionBox.Text?.Trim(),
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
