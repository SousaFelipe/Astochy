using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



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
