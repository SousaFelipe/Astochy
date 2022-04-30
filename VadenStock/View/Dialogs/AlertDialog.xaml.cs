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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace VadenStock.View.Dialogs
{
    public partial class AlertDialog : Border
    {
        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Danger,
            Error
        }

        public enum AlertIcon
        {
            Info,
            Success,
            Warning,
            Danger,
            Error
        }

        public enum AlertButton
        {
            None,
            Ok,
            Cancel,
            OkOrCancel,
            YesOrNo,
        }



        public string Message { get; private set; }
        public string Caption { get; private set; }
        public AlertType Type { get; private set; }
        public AlertIcon Icon { get; private set; }
        public AlertButton Button { get; private set; }



        public AlertDialog()
        {
            Message = "Message";
            Caption = "Caption";
            Type = AlertType.Info;
            Icon = AlertIcon.Info;
            Button = AlertButton.None;

            InitializeComponent();
        }
    }
}
