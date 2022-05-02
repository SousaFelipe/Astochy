using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

using VadenStock.Tools;



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



        static readonly float InitialPosition = -100;
        static readonly float DisplayPosition = 44;



        public AlertDialog()
        {
            InitializeComponent();

            Margin = new Thickness(0, InitialPosition, 0, 0);
        }



        public AlertDialog(AlertType type, string message, string caption = "Eitaa!")
        {
            InitializeComponent();

            Margin = new Thickness(0, InitialPosition, 0, 0);

            Loaded += delegate
            {
                _TextMessage.Text = message;
                _TextCaption.Text = caption;
                _ImageIcon.Source = Src.Icon(GetIconFromType(type));
                Background = new SolidColorBrush(GetBackgroundFromType(type));
            };
        }



        static string GetIconFromType(AlertType type)
        {
            return type switch
            {
                AlertType.Success => "white-checked",
                AlertType.Warning => "white-alert",
                AlertType.Danger => "white-bomb",
                AlertType.Info => "white-information",
                _ => "white-error"
            };
        }



        static Color GetBackgroundFromType(AlertType type)
        {
            return type switch
            {
                AlertType.Success => (Color)ColorConverter.ConvertFromString("#66BB6A"),
                AlertType.Warning => (Color)ColorConverter.ConvertFromString("#FFD740"),
                AlertType.Danger => (Color)ColorConverter.ConvertFromString("#FF7043"),
                AlertType.Info => (Color)ColorConverter.ConvertFromString("#40C4FF"),
                _ => (Color)ColorConverter.ConvertFromString("#AB47BC")
            };
        }



        public async Task Down()
        {
            double top = Margin.Top;

            while (Margin.Top < DisplayPosition)
            {
                top += 8;
                Margin = new Thickness(0, top, 0, 0);
                await Task.Delay(1);
            }
        }



        public async Task Up()
        {
            double top = Margin.Top;

            while (Margin.Top > InitialPosition)
            {
                top -= 8;
                Margin = new Thickness(0, top, 0, 0);
                await Task.Delay(1);
            }
        }



        private void ButtonClose_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseAlert(this, true);
        }
    }
}
