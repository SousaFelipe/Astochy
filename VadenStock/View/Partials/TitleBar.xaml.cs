using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;



namespace VadenStock.View.Partials
{
    public partial class TitleBar : UserControl
    {
        private static Window ParentWindow => Application.Current.MainWindow;



        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;

                return (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata
                    .DefaultValue;
            }
        }



        public TitleBar()
        {
            InitializeComponent();
        }



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if ( ! IsInDesignMode)
            {
                ParentWindow.StateChanged += new EventHandler(OnWindowStateChanged);
            }
        }



        private void OnMoveWindow(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.DragMove();
        }



        private void MinimizeWindow(object? sender, RoutedEventArgs e)
        {
            ParentWindow.WindowState = WindowState.Minimized;
        }



        private void ChangeWindowState(object? sender, RoutedEventArgs e)
        {
            ParentWindow.WindowState = (ParentWindow.WindowState == WindowState.Normal)
                ? WindowState.Maximized
                : WindowState.Normal;
        }



        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            string resourceKey = ParentWindow.WindowState == WindowState.Normal
                    ? "maximize"
                    : "restore";

            _ImageWindowStateControl.Source = new BitmapImage(
                    new($"/VadenStock;component/Resources/Icons/{resourceKey}.png", UriKind.Relative)
                );
        }



        private void ShutdownApplication(object? sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
