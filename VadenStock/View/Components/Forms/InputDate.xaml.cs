using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using VadenStock.Tools;



namespace VadenStock.View.Components.Forms
{
    public partial class InputDate : DatePicker
    {
        public int SelectCount { get; private set; }



        public InputDate()
        {
            InitializeComponent();
        }



        private void TextDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox input = (TextBox)sender;
            string text = input.Text;

            if (!string.IsNullOrEmpty(text))
            {
                Regex digits = new(@"[^0-9/]");
                input.Text = digits.Replace(text, "");
            }
        }



        private void TextDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
                return;
        }
    }
}
