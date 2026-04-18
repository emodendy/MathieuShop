using System;
using System.IO;

namespace MathieuShop.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Возвращает абсолютный путь к файлу ресурса относительно папки приложения.
        /// </summary>
        public static string GetAbsolutePath(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            // Базовая директория — папка запуска приложения
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, relativePath);
        }

        /// <summary>
        /// Возвращает URI для Avalonia Bitmap из относительного пути.
        /// </summary>
        public static string? GetImageUri(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var fullPath = GetAbsolutePath(relativePath);
            return File.Exists(fullPath) ? fullPath : null;
        }
    }
}
