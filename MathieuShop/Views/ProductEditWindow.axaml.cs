using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MathieuShop.Data;
using MathieuShop.Helpers;

namespace MathieuShop.Views
{
    public partial class ProductEditWindow : Window
    {
        private readonly int? productId;
        private string? selectedImagePath;

        public ProductEditWindow() : this(null) { }

        public ProductEditWindow(int? productId)
        {
            InitializeComponent();
            this.productId = productId;
            LoadCombos();

            if (productId.HasValue)
            {
                WindowTitle.Text = "Редактировать продукт";
                LoadProduct(productId.Value);
            }
            else
            {
                WindowTitle.Text = "Добавить продукт";
            }
        }

        private void LoadCombos()
        {
            var categories = App.DbContext.Categories.OrderBy(c => c.Name).ToList();
            CategoryCombo.ItemsSource = categories;
            CategoryCombo.DisplayMemberBinding = new Avalonia.Data.Binding("Name");

            var suppliers = App.DbContext.Suppliers.OrderBy(s => s.CompanyName).ToList();
            SupplierCombo.ItemsSource = suppliers;
            SupplierCombo.DisplayMemberBinding = new Avalonia.Data.Binding("CompanyName");

            CollectionCombo.ItemsSource = new[]
            {
                "Аниме", "Новый год", "Хэллоуин", "Киберпанк", "Нуар"
            };
        }

        private void LoadProduct(int id)
        {
            var product = App.DbContext.Products.Find(id);
            if (product == null) return;

            NameBox.Text = product.Name;
            PriceBox.Text = product.Price.ToString("F2");
            StockBox.Text = product.Stock.ToString();

            var categories = CategoryCombo.ItemsSource as System.Collections.Generic.List<Category>;
            CategoryCombo.SelectedItem = categories?.FirstOrDefault(c => c.CategoryId == product.CategoryId);

            var suppliers = SupplierCombo.ItemsSource as System.Collections.Generic.List<Supplier>;
            SupplierCombo.SelectedItem = suppliers?.FirstOrDefault(s => s.SupplierId == product.SupplierId);

            selectedImagePath = product.ImagePath;
            ImagePathBox.Text = product.ImagePath;
            LoadPreview(product.ImagePath);

            CollectionCombo.SelectedItem = product.Collection;
        }

        private void LoadPreview(string? relativePath)
        {
            var fullPath = ResourceHelper.GetImageUri(relativePath);
            if (fullPath != null)
            {
                try { PreviewImage.Source = new Bitmap(fullPath); }
                catch { PreviewImage.Source = null; }
            }
            else
            {
                PreviewImage.Source = null;
            }
        }

        private async void PickImageButton_Click(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerOpenOptions
            {
                Title = "Выберите изображение",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp" }
                    }
                }
            };

            var files = await StorageProvider.OpenFilePickerAsync(options);
            if (files.Count == 0) return;

            var fullPath = files[0].Path.LocalPath;

            // Try to make path relative to app base dir
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (fullPath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                selectedImagePath = fullPath[baseDir.Length..].Replace('\\', '/');
            else
                selectedImagePath = fullPath;

            ImagePathBox.Text = selectedImagePath;
            LoadPreview(selectedImagePath);
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            { ShowError("Введите название."); return; }

            if (!decimal.TryParse(PriceBox.Text?.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal price) || price < 0)
            { ShowError("Введите корректную цену."); return; }

            if (!int.TryParse(StockBox.Text, out int stock) || stock < 0)
            { ShowError("Введите корректный остаток."); return; }

            if (CategoryCombo.SelectedItem is not Category category)
            { ShowError("Выберите категорию."); return; }

            if (SupplierCombo.SelectedItem is not Supplier supplier)
            { ShowError("Выберите поставщика."); return; }

            if (productId.HasValue)
            {
                var entity = App.DbContext.Products.Find(productId.Value);
                if (entity != null)
                {
                    entity.Name = NameBox.Text.Trim();
                    entity.Price = price;
                    entity.Stock = stock;
                    entity.CategoryId = category.CategoryId;
                    entity.SupplierId = supplier.SupplierId;
                    entity.ImagePath = selectedImagePath;
                    entity.Collection = CollectionCombo.SelectedItem as string;
                    entity.UpdatedAt = DateTime.UtcNow;
                    App.DbContext.SaveChanges();
                }
            }
            else
            {
                var newProduct = new Product
                {
                    Name = NameBox.Text.Trim(),
                    Price = price,
                    Stock = stock,
                    CategoryId = category.CategoryId,
                    SupplierId = supplier.SupplierId,
                    ImagePath = selectedImagePath,
                    Collection = CollectionCombo.SelectedItem as string,
                    UpdatedAt = DateTime.UtcNow
                };
                App.DbContext.Products.Add(newProduct);
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
