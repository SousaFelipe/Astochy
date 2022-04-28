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



        public static string Currency(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "0,00";

            char[] input = Number(value).ToCharArray();
            char[] output = new char[(input.Length >= 3) ? input.Length : 3];

            if (input.Length < output.Length)
            {
                //for (int i = output.Length - 1; i >= 0; i--)
                //    output[i] = (i > input.Length - 1) ? '0' : input[i];
                for (int i = output.Length - 1; i >= 0; i--)
                    output[i] = ((i - (i - output.Length - input.Length)) >= 0) ? input[i] : '0';
            }

            return new string(output).Insert(1, ",");

            //for (int i = output.Length - 1; i >= 0; i--)
            //{

            //}

            //---------------
            //| i | in | ou |
            //---------------

            // 1            0,01
            // 12           0,12
            // 123          01,23
            // 1234         12,34
            // 12345        123,45
            // 123456       1.234,56
            // 1234567      12.345,67
            // 12345678     123.456,78
        }
    }
}
