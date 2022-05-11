using System.Windows.Media;



namespace VadenStock.Tools
{
    public static class Clr
    {
        public static SolidColorBrush Color(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }
    }
}
