using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace VadenStock.View.Components.Forms
{
    public partial class InputCurrency : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
                "Placeholder",
                typeof(string),
                typeof(InputCurrency),
                new UIPropertyMetadata(string.Empty, PlaceholderPropertyCallback)
            );



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }



        public double Currency { get; set; }



        public static void PlaceholderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            InputCurrency instance = (InputCurrency)root;
            instance.Placeholder = (string)e.NewValue;
        }



        public InputCurrency()
        {
            InitializeComponent();
        }



        private void OnTypeText(object sender, KeyEventArgs e)
        {
            InputCurrency input = (InputCurrency)sender;

            if (e.Key == Key.Left || e.Key == Key.Right)
                return;

            if (e.Key >= Key.D0 || e.Key <= Key.D9)
            {
                if (input.Text.Length > 0)
                {
                    /*string[] str = input.Text.Split(',');
                    string clean = Utils.Number(str.Length > 0 ? str[0] : input.Text);

                    if (double.TryParse(clean, out double value))
                    {
                        input.Text = string.Format("{0,1:N2}", value).TrimStart();
                        input.Currency = (value > 0) ? (value / 100) : 0;

                        input.SelectionStart = ((input.Text.Length - 1) - 2);
                        input.SelectionLength = 0;
                    }*/

                    string output = Utils.Currency(input.Text);
                    System.Diagnostics.Trace.WriteLine($"Output: { output }");
                }
            }
        }



        private void OnChageSelection(object sender, RoutedEventArgs e)
        {
            //InputCurrency input = (InputCurrency)sender;

            //System.Diagnostics.Trace.WriteLine(input.SelectionStart);
        }
    }
}
