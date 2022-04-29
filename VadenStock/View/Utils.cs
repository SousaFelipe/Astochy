using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
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

            public static string GetFileType(string filename)
            {
                string[] pices = filename.Split('.');

                if (pices.Length > 0)
                {
                    return pices[1] switch
                    {
                        "png" => FileType.PNG,
                        "jpg" => FileType.JPG,
                        "jpeg" => FileType.JPEG,
                        _ => string.Empty
                    };
                }

                return string.Empty;
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



        public static string ZeroFill(int number, string? concat = "")
        {
            string zero = (number > 0 && number < 10)
                ? $"0{ number }"
                : number.ToString();

            return string.Concat(zero, concat);
        }



        public static string Number(this string dirty)
        {
            string clean = string.Empty;

            for (int i = 0; i < dirty.Length; i++)
            {
                if (Char.IsDigit(dirty[i]))
                    clean += dirty[i];
            }

            return clean;
        }



        public static string Currency(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "0,00";

            string output = Sanitize(input, 3);
                   output = output.Insert(output.Length - 2, ",");

            if (output.Length > 4)
            {
                for (int i = output.Length - 1; i >= 0; i--)
                {

                }
            }

            // 123          1,23
            // 1234         12,34
            // 12345        123,45
            // 123466       1.234,56
            // 1234567      12.345,67
            // 12345678     123.456,78
            // 123456789    1.234.567,89

            return new string(output);
        }



        public static string Sanitize(string value, int fill = 3)
        {
            string input = Number(value);
            char[] output = new char[(input.Length >= fill) ? input.Length : fill];

            if (input.Length < output.Length)
            {
                int pos;

                for (int i = output.Length - 1; i >= 0; i--)
                {
                    pos = (i - (output.Length - input.Length));
                    output[i] = (pos >= 0) ? input[pos] : '0';
                }
            }
            else
            {
                for (int i = output.Length - 1; i >= 0; i--)
                {
                    output[i] = input[i];
                }
            }

            return new string(output);
        }
    }
}
