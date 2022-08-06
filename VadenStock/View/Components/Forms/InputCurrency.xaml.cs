using System;
using System.Collections.Generic;
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
            List<char> output = new();

            string input = Str.Sanitize(Text);
            string outst;

            if (string.IsNullOrEmpty(input))
                outst = "0,00";
            
            else
            {
                int  z = (input[0] == '0') ? 3 : input.Length;
                int  c = (input.Length - (z + 1));

                for (; ((input[0] == '0') ? (c < (input.Length - 1)) : (c < input.Length)); c++)
                {
                    if ((c + 1) < input.Length && input[c + 1] != '0')
                        output.Add(input[c + 1]);

                    else if (input[0] == '0' && c >= 0 && c < input.Length)
                        output.Add(input[c]);
                }

                outst = new(output.ToArray());
                outst = outst.Insert(outst.Length - 2, ",");
            }

            Text = Str.Currency(Convert.ToDouble(outst));
            CaretIndex = Text.Length;
        }



        private void OnChageSelection(object sender, RoutedEventArgs e)
        {
            InputCurrency input = (InputCurrency)sender;

            if (input.Text.Length > 0 && input.SelectionStart < input.Text.Length)
                input.SelectionStart = input.Text.Length;
        }
    }
}
