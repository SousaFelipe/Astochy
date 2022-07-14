using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Tools;



namespace VadenStock.View.Components.Widgets
{
    public class Icon
    {
        public static Image Small(string name, string color = "black")
        {
            return new Image()
            {
                Width = 18,
                Height = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Source = Src.Icon($"{color}-{name}")
            };
        }



        public static Image Medium(string name, string color = "black")
        {
            return new Image()
            {
                Width = 24,
                Height = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Source = Src.Icon($"{color}-{name}")
            };
        }



        public static Image Large(string name, string color = "black")
        {
            return new Image()
            {
                Width = 32,
                Height = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Source = Src.Icon($"{color}-{name}")
            };
        }
    }
}
