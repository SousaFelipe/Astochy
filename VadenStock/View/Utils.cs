using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media.Imaging;



namespace VadenStock.View
{
    public static class Utils
    {
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



        public static BitmapImage FindStorageImage(string filename)
        {
            Stream stream = File.OpenRead($"{ Directory.GetCurrentDirectory() }\\Resources\\Storage\\{ filename }");
            BitmapImage img = new();

            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();

            return img;
        }



        public static string ZeroFill(int number, string? concat = "")
        {
            string zero = (number > 0 && number < 10)
                ? $"0{ number }"
                : number.ToString();

            return string.Concat(zero, concat);
        }
    }
}
