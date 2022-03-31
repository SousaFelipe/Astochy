using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;



namespace VadenStock.View.Partials
{
    public partial class TitleBar : UserControl
    {
        private Window? ParentWindow { get; set; }



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
                ParentWindow = Application.Current.MainWindow;
                ParentWindow.StateChanged += new EventHandler(OnWindowStateChanged);
            }
        }



        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            if (ParentWindow != null)
            {
                string resourceKey = ParentWindow.WindowState == WindowState.Normal
                    ? "maximize"
                    : "restore";

                ImageWindowStateControl.Source = new BitmapImage(
                    new($"/VadenStock;component/Resources/Icons/{resourceKey}.png", UriKind.Relative)
                );
            }
        }



        private void MinimizeWindow(object? sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
                ParentWindow.WindowState = WindowState.Minimized;
        }



        private void ChangeWindowState(object? sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
                ParentWindow.WindowState = (ParentWindow.WindowState == WindowState.Normal)
                    ? WindowState.Maximized
                    : WindowState.Normal;
        }



        private void ShutdownApplication(object? sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
