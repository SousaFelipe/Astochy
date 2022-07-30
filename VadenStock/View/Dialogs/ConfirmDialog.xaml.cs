using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ConfirmDialog : Border
    {
        public enum ConfirmType
        {
            Info,
            Question,
            Delete
        }



        private Action<object>? confirm;



        public ConfirmDialog()
        {
            this.confirm = null;

            InitializeComponent();
        }



        public ConfirmDialog(ConfirmType type, string message)
        {
            this.confirm = null;

            InitializeComponent();

            Loaded += delegate
            {
                _ImageIcon.Source = Src.Image(GetIcon(type));
                _TextMessage.Text = message;

                if (type == ConfirmType.Delete)
                {
                    _GridButtonsTwoo.Visibility = Visibility.Collapsed;
                    _GridButtonsOne.Visibility = Visibility.Visible;
                }
                else
                {
                    _GridButtonsOne.Visibility = Visibility.Collapsed;
                    _GridButtonsTwoo.Visibility = Visibility.Visible;
                }
            };
        }



        private static string GetIcon(ConfirmType type)
        {
            return type switch
            {
                ConfirmType.Info => "64-info",
                ConfirmType.Question => "64-question",
                ConfirmType.Delete => "64-remove",
                _ => "64-question"
            };
        }



        public ConfirmDialog OnConfirm(Action<object> confirm)
        {
            this.confirm = confirm;
            return this;
        }



        private void CallConfirm(object sender, RoutedEventArgs e)
        {
            this.confirm?.Invoke(sender);
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
