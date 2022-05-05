using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;



namespace VadenStock.View.Components.Containers
{
    public class Header
    {
        public enum Width
        {
            Auto,
            Max
        }



        public string? Title { get; set; }   
        public Width W { get; set; }



        public static Header Auto(string title)
        {
            return new Header()
            {
                Title = title,
                W = Width.Auto
            };
        }



        public static Header Max(string title)
        {
            return new Header()
            {
                Title = title,
                W = Width.Max
            };
        }
    }
}
