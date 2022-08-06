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
            string input = Str.Sanitize(Text);
            List<char> output = new();

            if (!string.IsNullOrEmpty(input))
            {
                int  z = (input[0] == '0') ? 3 : input.Length;
                int  c = (input.Length - (z + 1));

                for (; ((input[0] == '0') ? (c < (input.Length - 1)) : (c < input.Length)); c++)
                {
                    if ((c + 1) < input.Length && input[c + 1] != '0')
                        output.Add(input[c + 1]);

                    else if (input[0] == '0' && c < input.Length)
                        output.Add(input[c]);
                }
            }

            string outstr = new(output.ToArray());
            outstr = outstr.Insert(outstr.Length - 2, ",");

            Text = Str.Currency(Convert.ToDouble(outstr));
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
