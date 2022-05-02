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
                goto exit_whitout_create;

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

            exit_whitout_create:
                return true;
        }



        public static bool CopyToResource(string from, string to, string fileName)
        {
            if (Directory.Exists(Resource.Root))
            {
                try
                {
                    string filePath = $"{ to }\\{ fileName }";

                    File.Copy(
                            from,
                            filePath,
                            true
                        );

                    return File.Exists(filePath);
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

            if (parts != null && (parts.Length - 1) >= 0)
                parts[^1] = to;

            return string.Join("\\", parts);
        }



        public static BitmapImage? OpenBitmap(string filePath)
        {
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



        public static BitmapImage? Icon(string name)
        {
            string path = $"{ Resource.Root }\\{ Resource.Icons }\\";
            return OpenBitmap($"{ path }\\{ name }.png");
        }



        public static BitmapImage? Storage(string name)
        {
            string path = $"{ Resource.Root }\\{ Resource.Storage }\\";
            return OpenBitmap($"{ path }\\{ name }");
        }
    }
}
