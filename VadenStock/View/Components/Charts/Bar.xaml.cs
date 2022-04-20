using System;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;



namespace VadenStock.View.Components.Charts
{
    public partial class Bar : UserControl
    {
        public static readonly DependencyProperty OrientationProp = DependencyProperty.Register(
                "Orientation",
                typeof(string),
                typeof(Bar),
                new UIPropertyMetadata("Horizontal")
            );

        public static readonly DependencyProperty CurrentWidthProp = DependencyProperty.Register(
                "CurrentWidth",
                typeof(double),
                typeof(Bar),
                new UIPropertyMetadata(0.0, CurrentWidthCallback)
            );

        public static readonly DependencyProperty CurrentHeightProp = DependencyProperty.Register(
                "CurrentHeight",
                typeof(double),
                typeof(Bar),
                new UIPropertyMetadata(0.0, CurrentHeightCallback)
            );

        public static readonly DependencyProperty BorderRadiusProp = DependencyProperty.Register(
                "BorderRadius",
                typeof(double),
                typeof(Bar),
                new UIPropertyMetadata(0.0, BorderRadiusCallback)
            );

        public static readonly DependencyProperty BackColorProp = DependencyProperty.Register(
                "BackColor",
                typeof(object),
                typeof(Bar),
                new UIPropertyMetadata("#ECEFF1", BackColorCallback)
            );

        public static readonly DependencyProperty BarColorProp = DependencyProperty.Register(
                "BarColor",
                typeof(object),
                typeof(Bar),
                new UIPropertyMetadata("#40C4FF", BarColorCallback)
            );


       
        public string Orientation
        {
            get { return (string)GetValue(OrientationProp); }
            set { SetValue(OrientationProp, value); }
        }



        public bool IsHorizontal { get { return Orientation == "Horizontal"; } }



        public double CurrentWidth
        {
            get { return (double)GetValue(IsHorizontal ? CurrentWidthProp : CurrentHeightProp); }
            set { SetValue(IsHorizontal ? CurrentWidthProp : CurrentHeightProp, value); }
        }

        public double CurrentHeight
        {
            get { return (double)GetValue(IsHorizontal ? CurrentHeightProp : CurrentWidthProp); }
            set { SetValue(IsHorizontal ? CurrentHeightProp : CurrentWidthProp, value); }
        }

        public double BorderRadius
        {
            get { return (double)GetValue(BorderRadiusProp); }
            set { SetValue(BorderRadiusProp, value); }
        }

        public object BackColor
        {
            get { return GetValue(BackColorProp); }
            set { SetValue(BackColorProp, value); }
        }

        public object BarColor
        {
            get { return GetValue(BarColorProp); }
            set { SetValue(BarColorProp, value); }
        }



        public static void CurrentWidthCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Bar bar = (Bar)root;
            bar._BorderContent.Width = (double)e.NewValue;
        }

        public static void CurrentHeightCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Bar bar = (Bar)root;
            bar._BorderContent.Height = (double)e.NewValue;
        }

        public static void BorderRadiusCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Bar bar = (Bar)root;
            bar._BorderContent.CornerRadius = new CornerRadius((double)e.NewValue);
            bar._Bar.CornerRadius = new CornerRadius((double)e.NewValue);
        }

        public static void BackColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Bar bar = (Bar)root;
            bar._BorderContent.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString((string)e.NewValue)
                );
        }

        public static void BarColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Bar bar = (Bar)root;
            bar._Bar.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString((string)e.NewValue)
                );
        }



        public Bar()
        {
            InitializeComponent();
            Initialize();
        }



        public Bar(System.Windows.Controls.Orientation o = System.Windows.Controls.Orientation.Horizontal)
        {
            InitializeComponent();

            Orientation = (o == System.Windows.Controls.Orientation.Horizontal)
                    ? "Horizontal"
                    : "Vertical";

            Initialize();
        }



        private void Initialize()
        {
            Loaded += delegate
            {
                _Bar.VerticalAlignment = IsHorizontal ? VerticalAlignment.Stretch : VerticalAlignment.Bottom;
                _Bar.HorizontalAlignment = IsHorizontal ? HorizontalAlignment.Left : HorizontalAlignment.Stretch;
            };
        }



        public void UpdateBar(double percent, double border = 0.0)
        {
            if (_Bar != null)
            {
                if (IsHorizontal)
                    _Bar.Width = ((_BorderContent.ActualWidth - border) * percent) / 100;
                else
                    _Bar.Height = ((_BorderContent.ActualHeight - border) * percent) / 100;
            }
        }
    }
}
