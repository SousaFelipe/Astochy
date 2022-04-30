using System;
using System.IO;
using System.Windows.Media.Imaging;



namespace VadenStock.Tools
{
    public static class Src
    {
        public struct Resource
        {
            public static string Root { get { return $"{ Directory.GetCurrentDirectory() }\\Resources";  } }
            public static string Storage { get { return "Storage"; } }
            public static string Icons { get { return "Icons"; } }

            public struct FileType
            {
                public static readonly string PNG = "png";
                public static readonly string JPG = "jpg";
                public static readonly string JPEG = "jpeg";
            }
        }



        public static bool CreateSorcePath()
        {
            if (Directory.Exists(Resource.Root))
                return true;

            try
            {
                Directory.CreateDirectory(Resource.Root);
                Directory.CreateDirectory($"{ Resource.Root }\\{ Resource.Icons }");
                Directory.CreateDirectory($"{ Resource.Root }\\{ Resource.Storage }");

                return Directory.Exists(Resource.Root);
            }
            catch (IOException e)
            {
                System.Diagnostics.Trace.WriteLine($"[VADEN]: { e.Message }");
                return false;
            }
        }



        public static bool CopyToResource(string from, string to, string fileName = "")
        {
            if (Directory.Exists(Resource.Root))
            {
                try
                {
                    if (!string.IsNullOrEmpty(fileName))
                        to = RenameFile(to, fileName);

                    File.Copy(from, to, true);

                    return File.Exists(to);
                }
                catch (IOException e)
                {
                    System.Diagnostics.Trace.WriteLine($"[VADEN]: { e.Message }");
                    return false;
                }
            }

            return false;
        }



        public static string RenameFile(string from, string to)
        {
            string[] parts = from.Split('\\');
            int namePosition = (parts.Length - 1);

            if (parts != null && namePosition >= 0)
                parts[namePosition] = to;

            return String.Join("\\", parts);
        }



        public static BitmapImage? OpenBitmap(string filePath)
        {
            // $"{ Directory.GetCurrentDirectory() }\\Resources\\{ completeFileName }"
            /*string completeFileName = string.IsNullOrEmpty(Path.GetExtension(filePath))
                ? string.IsNullOrEmpty(Path.GetExtension(fileName))
                    ? $"{ filePath }\\{ fileName }.{ fileType }"
                    : $"{ filePath }\\{ fileName }"
                : filePath;*/

            try
            {
                Stream stream = File.OpenRead(filePath);
                BitmapImage img = new();

                img.BeginInit();
                img.StreamSource = stream;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                img.Freeze();

                return img;
            }
            catch (IOException e)
            {
                System.Diagnostics.Trace.WriteLine($"[VADEN] { e.Message }");
                return null;
            }
        }
    }
}
