using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Dialogs
{
    public partial class ConfirmDialog : Border
    {
        public Func<bool>? Confirm;



        public ConfirmDialog()
        {
            Confirm = null;

            InitializeComponent();
        }



        private void CallConfirm(object sender, RoutedEventArgs e)
        {
            Confirm?.Invoke();
            Close();
        }



        private void CallCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }



        public void Close()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
