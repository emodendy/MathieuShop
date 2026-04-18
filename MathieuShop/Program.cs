using System;
using Avalonia;

namespace MathieuShop
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Fix: Npgsql 6+ requires UTC DateTimes.
            // This switch enables legacy behaviour that accepts Local/Unspecified kinds.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
