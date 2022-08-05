using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using VadenStock.Tools;



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

            if (e.Key >= Key.D0 || e.Key <= Key.D9)
            {
                string output = Str.Currency(input.Text);
                input.Text = output;
            }

            if (input.Text.Length > 0)
                input.SelectionStart = input.Text.Length;
        }



        private void OnChageSelection(object sender, RoutedEventArgs e)
        {
            InputCurrency input = (InputCurrency)sender;

            if (input.Text.Length > 0 && input.SelectionStart < input.Text.Length)
                input.SelectionStart = input.Text.Length;
        }
    }
}
