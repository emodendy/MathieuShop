using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MathieuShop.Data;
using MathieuShop.Helpers;
using MathieuShop.Views;

namespace MathieuShop
{
    public partial class MainWindow : Window
    {
        private ProductsPage? productsPage;

        public MainWindow()
        {
            InitializeComponent();
            LoadLogo();
            LoadCollectionCombo();

            // Wire up nav buttons manually (compiled bindings limitation)
            NavServicesButton.Click += (_, _) => NavServicesButton_Click(null, null!);
            NavExitButton.Click     += (_, _) => NavExitButton_Click(null, null!);

            productsPage = new ProductsPage(this);
            MainContent.Content = productsPage;
        }

        private void LoadLogo()
        {
            var logoPath = ResourceHelper.GetImageUri("Ресурсы/Logo.png");
            if (logoPath != null)
            {
                LogoImage.Source = new Bitmap(logoPath);
                LogoPlaceholder.IsVisible = false;
            }
        }

        private void LoadCollectionCombo()
        {
            var items = new System.Collections.Generic.List<string>
            {
                "Все",
                "Аниме",
                "Новый год",
                "Хэллоуин",
                "Киберпанк",
                "Нуар"
            };
            CollectionBox.ItemsSource = items;
            CollectionBox.SelectedIndex = 0;
        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private async void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog("Закрыть приложение?");
            var result = await dialog.ShowDialog<bool>(this);
            if (result) Close();
        }

        // ── Right panel ──

        private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (MainContent.Content is ProductsPage pp)
                pp.SetSearch(SearchBox.Text ?? string.Empty);
        }

        private void CollectionBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MainContent.Content is not ProductsPage pp) return;

            var selected = CollectionBox.SelectedItem as string;
            pp.FilterByCollection(selected == "Все" ? null : selected);
        }

        private void SortAlphaButton_Click(object? sender, RoutedEventArgs e)
        {
            if (MainContent.Content is not ProductsPage pp) return;
            pp.ToggleSort();
            SortAlphaButton.Content = pp.SortAscending ? "А → Я" : "Я → А";
        }

        private void PrevPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (MainContent.Content is ProductsPage pp)
                pp.PrevPage();
        }

        private void NextPageButton_Click(object? sender, RoutedEventArgs e)
        {
            if (MainContent.Content is ProductsPage pp)
                pp.NextPage();
        }

        // ── Left sidebar ──

        private void NavServicesButton_Click(object? sender, RoutedEventArgs e)
        {
            productsPage = new ProductsPage(this);
            MainContent.Content = productsPage;
        }

        private async void NavExitButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog("Выйти из аккаунта?");
            var result = await dialog.ShowDialog<bool>(this);
            if (result)
            {
                CurrentSession.CurrentUser = null;
                var loginWindow = new Views.LoginWindow();
                loginWindow.Show();
                Close();
            }
        }

        public void UpdatePageInfo(string text) { }
    }
}
