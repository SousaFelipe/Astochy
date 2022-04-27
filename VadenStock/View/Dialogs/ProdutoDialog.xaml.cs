using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Dialogs
{
    public partial class ProdutoDialog : Window
    {
        public ProdutoDialog()
        {
            InitializeComponent();
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VadenStock.MainWindow window = (VadenStock.MainWindow)Application.Current.MainWindow;
            window.ExitDialogMode();
        }
    }
}
