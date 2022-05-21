using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace VadenStock.View.Components
{
    public partial class Text : TextBlock
    {
        public static readonly object ColorPrimary   = ColorConverter.ConvertFromString("#263238");
        public static readonly object ColorSecondary = ColorConverter.ConvertFromString("#607D8B");
        public static readonly object ColorTertiary  = ColorConverter.ConvertFromString("#78909C");
        public static readonly object ColorMuted     = ColorConverter.ConvertFromString("#CFD8DC");



        public Text(string content)
        {
            InitializeComponent();

            Loaded += delegate
            {
                Text = content;
                Foreground = new SolidColorBrush((Color)ColorPrimary);
                FontSize = 13;
            };
        }



        public Text(string content, double fontSize)
        {
            InitializeComponent();

            Loaded += delegate
            {
                Text = content;
                FontSize = fontSize;
                Foreground = new SolidColorBrush((Color)ColorPrimary);
            };
        }



        public Text(string content, double fontSize, object foreground)
        {
            InitializeComponent();

            Loaded += delegate
            {
                Text = content;
                FontSize = fontSize;
                Foreground = new SolidColorBrush((Color)foreground);
            };
        }
    }
}
