using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace VadenStock.View
{
    public static class Utils
    {
        public struct Resource
        {
            public static string Storage { get { return "Storage";  } }
            public static string Icons { get { return "Icons";  } }

            public struct FileType
            {
                public static readonly string PNG = "png";
                public static readonly string JPG = "jpg";
                public static readonly string JPEG = "jpeg";
            }
        }



        public static T? GetChildOfType<T>(this DependencyObject dep) where T : DependencyObject
        {
            if (dep == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                var result = (child as T) ?? GetChildOfType<T>(child);

                if (result != null)
                    return result;
            }

            return null;
        }



        public static BitmapImage FindResource(string fileName, string filePath, string filetype = "")
        {
            string completeFileName = string.IsNullOrEmpty(Path.GetExtension(fileName))
                    ? $"{ fileName }.{ filetype }"
                    : fileName;

            Stream stream = File.OpenRead($"{ Directory.GetCurrentDirectory() }\\Resources\\{ filePath }\\{ completeFileName }");
            BitmapImage img = new();

            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();

            return img;
        }
    }
}
