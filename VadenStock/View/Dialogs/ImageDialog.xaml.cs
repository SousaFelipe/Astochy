using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;




namespace VadenStock.View.Dialogs
{
    public partial class ImageDialog : Border
    {
        public ImageDialog()
        {
            InitializeComponent();
        }



        public ImageDialog(Brush imageBrush)
        {
            InitializeComponent();

            Loaded += delegate
            {
                _BorderImage.Background = imageBrush;
            };
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
