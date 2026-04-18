using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MathieuShop.Data;
using MathieuShop.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MathieuShop.Views
{
    public partial class ProductsPage : UserControl
    {
        private readonly MainWindow? mainWindow;
        private List<Product> allProducts = new();
        private List<Product> filteredProducts = new();
        private int currentPage = 1;
        private const int pageSize = 3;
        private string? categoryFilter = null;   // Кастом / Косплей
        private string? collectionFilter = null; // Аниме / Новый год / etc.
        private string searchText = string.Empty;

        public ProductsPage() : this(null) { }

        public ProductsPage(MainWindow? mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            LoadData();
            SetActiveTab(TabKastomButton, TabKosplayButton);
        }

        public void LoadData()
        {
            allProducts = App.DbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();
            ApplyFiltersAndRender();
        }

        public void SetSearch(string text)
        {
            searchText = text;
            currentPage = 1;
            ApplyFiltersAndRender();
        }

        // Фильтр по категории (вкладки Кастом/Косплей)
        public void FilterByCategory(string? categoryName)
        {
            categoryFilter = categoryName;
            currentPage = 1;
            ApplyFiltersAndRender();
        }

        // Фильтр по коллекции (ComboBox справа)
        public void FilterByCollection(string? collection)
        {
            collectionFilter = collection;
            currentPage = 1;
            ApplyFiltersAndRender();
        }

        public bool SortAscending { get; private set; } = true;

        public void ToggleSort()
        {
            SortAscending = !SortAscending;
            currentPage = 1;
            ApplyFiltersAndRender();
        }

        public void PrevPage()
        {
            if (currentPage > 1) { currentPage--; ApplyFiltersAndRender(); }
        }

        public void NextPage()
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(filteredProducts.Count / (double)pageSize));
            if (currentPage < totalPages) { currentPage++; ApplyFiltersAndRender(); }
        }

        private void ApplyFiltersAndRender()
        {
            var query = allProducts
                .Where(p =>
                    (categoryFilter == null || p.Category?.Name == categoryFilter) &&
                    (collectionFilter == null || p.Collection == collectionFilter) &&
                    (string.IsNullOrEmpty(searchText) ||
                     p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));

            filteredProducts = SortAscending
                ? query.OrderBy(p => p.Name).ToList()
                : query.OrderByDescending(p => p.Name).ToList();

            int total = filteredProducts.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            int from = total == 0 ? 0 : (currentPage - 1) * pageSize + 1;
            int to = Math.Min(currentPage * pageSize, total);
            PageInfoText.Text = $"{from}-{to} из {total}";

            RenderCards(filteredProducts
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList());
        }

        private void RenderCards(List<Product> products)
        {
            CardsPanel.Children.Clear();
            foreach (var product in products)
                CardsPanel.Children.Add(BuildCard(product));
        }

        private Border BuildCard(Product product)
        {
            // ── Photo ──
            var photoBox = new Border
            {
                Width = 120,
                Height = 100,
                BorderBrush = Brush.Parse("#E0E0E0"),
                BorderThickness = new Thickness(1),
                Background = Brush.Parse("#F8F8F8"),
                Margin = new Thickness(14, 14, 16, 14),
                CornerRadius = new CornerRadius(4),
                ClipToBounds = true
            };

            var imgPath = ResourceHelper.GetImageUri(product.ImagePath);
            if (imgPath != null)
            {
                try
                {
                    photoBox.Child = new Image
                    {
                        Source = new Bitmap(imgPath),
                        Stretch = Stretch.UniformToFill
                    };
                }
                catch { photoBox.Child = PhotoPlaceholder(); }
            }
            else
            {
                photoBox.Child = PhotoPlaceholder();
            }

            // ── Name + badge ──
            var nameRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Thickness(0, 0, 0, 4)
            };

            nameRow.Children.Add(new TextBlock
            {
                Text = product.Name,
                FontSize = 14,
                FontWeight = FontWeight.SemiBold,
                Foreground = Brush.Parse("#1A1A1A"),
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 260
            });

            if (!string.IsNullOrEmpty(product.Collection))
            {
                nameRow.Children.Add(new Border
                {
                    Background = Brush.Parse("#EEF2FF"),
                    BorderBrush = Brush.Parse("#C7D2FE"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(7, 2),
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new TextBlock
                    {
                        Text = product.Collection,
                        FontSize = 10,
                        Foreground = Brush.Parse("#4F46E5")
                    }
                });
            }

            // ── Details grid ──
            var details = new Grid();
            details.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            details.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            details.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            details.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            details.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            void AddDetail(int row, string label, string value, string color = "#555555")
            {
                var lbl = new TextBlock
                {
                    Text = label,
                    FontSize = 11,
                    Foreground = Brush.Parse("#999999"),
                    Margin = new Thickness(0, 1, 8, 1)
                };
                var val = new TextBlock
                {
                    Text = value,
                    FontSize = 11,
                    Foreground = Brush.Parse(color),
                    FontWeight = FontWeight.Medium
                };
                Grid.SetRow(lbl, row); Grid.SetColumn(lbl, 0);
                Grid.SetRow(val, row); Grid.SetColumn(val, 1);
                details.Children.Add(lbl);
                details.Children.Add(val);
            }

            AddDetail(0, "Цена:", $"{product.Price:N2} ₽", "#1A1A1A");
            AddDetail(1, "Остаток:", $"{product.Stock} шт.");
            AddDetail(2, "Категория:", product.Category?.Name ?? "—");

            // ── Buttons ──
            var editBtn = new Button
            {
                Content = "✏ Изменить",
                Height = 26,
                Padding = new Thickness(10, 0),
                Background = Brush.Parse("#EFF6FF"),
                Foreground = Brush.Parse("#2563EB"),
                BorderBrush = Brush.Parse("#BFDBFE"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                FontSize = 11,
                Tag = product
            };
            editBtn.Click += EditCard_Click;

            var deleteBtn = new Button
            {
                Content = "✕ Удалить",
                Height = 26,
                Padding = new Thickness(10, 0),
                Background = Brush.Parse("#FEF2F2"),
                Foreground = Brush.Parse("#DC2626"),
                BorderBrush = Brush.Parse("#FECACA"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                FontSize = 11,
                Tag = product
            };
            deleteBtn.Click += DeleteCard_Click;

            var btnRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Margin = new Thickness(0, 8, 0, 0)
            };
            btnRow.Children.Add(editBtn);
            btnRow.Children.Add(deleteBtn);

            // ── Info panel ──
            var infoPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 14, 14, 14),
                Spacing = 0
            };
            infoPanel.Children.Add(nameRow);
            infoPanel.Children.Add(details);
            infoPanel.Children.Add(btnRow);

            // ── Updated at (bottom right) ──
            var updatedText = new TextBlock
            {
                Text = $"Обновлено: {product.UpdatedAt:dd.MM.yyyy HH:mm}",
                FontSize = 10,
                Foreground = Brush.Parse("#BBBBBB"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 14, 8)
            };

            var rowGrid = new Grid();
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            rowGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            rowGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            Grid.SetColumn(photoBox, 0);
            Grid.SetRowSpan(photoBox, 2);
            Grid.SetColumn(infoPanel, 1);
            Grid.SetRow(updatedText, 1);
            Grid.SetColumn(updatedText, 1);

            rowGrid.Children.Add(photoBox);
            rowGrid.Children.Add(infoPanel);
            rowGrid.Children.Add(updatedText);

            return new Border
            {
                BorderBrush = Brush.Parse("#EEEEEE"),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Background = Brush.Parse("White"),
                Child = rowGrid
            };
        }

        private static TextBlock PhotoPlaceholder() => new TextBlock
        {
            Text = "Фото",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brush.Parse("#CCCCCC"),
            FontSize = 12
        };

        // ── Tabs ──

        private void TabKastomButton_Click(object? sender, RoutedEventArgs e)
        {
            FilterByCategory("Кастом");
            SetActiveTab(TabKastomButton, TabKosplayButton);
        }

        private void TabKosplayButton_Click(object? sender, RoutedEventArgs e)
        {
            FilterByCategory("Косплей");
            SetActiveTab(TabKosplayButton, TabKastomButton);
        }

        private void SetActiveTab(Button active, Button inactive)
        {
            active.Background = Brush.Parse("White");
            active.BorderThickness = new Thickness(1, 1, 1, 0);
            inactive.Background = Brush.Parse("#E8E8E8");
            inactive.BorderThickness = new Thickness(0, 1, 1, 0);
        }

        // ── CRUD ──

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var window = new ProductEditWindow(null);
            var parent = this.VisualRoot as Window;
            await window.ShowDialog(parent!);
            LoadData();
        }

        private async void EditCard_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                var window = new ProductEditWindow(product.ProductId);
                var parent = this.VisualRoot as Window;
                await window.ShowDialog(parent!);
                LoadData();
            }
        }

        private async void DeleteCard_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                var dialog = new ConfirmDialog($"Удалить \"{product.Name}\"?");
                var parent = this.VisualRoot as Window;
                var result = await dialog.ShowDialog<bool>(parent!);
                if (result)
                {
                    var entity = App.DbContext.Products.Find(product.ProductId);
                    if (entity != null)
                    {
                        App.DbContext.Products.Remove(entity);
                        App.DbContext.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}
