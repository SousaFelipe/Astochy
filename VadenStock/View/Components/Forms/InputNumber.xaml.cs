using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VadenStock.Tools;

namespace VadenStock.View.Components.Forms
{
    public partial class InputNumber : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
                "Placeholder",
                typeof(string),
                typeof(InputNumber),
                new UIPropertyMetadata(string.Empty, PlaceholderPropertyCallback)
            );



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }



        public static void PlaceholderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            InputNumber instance = (InputNumber)root;
            instance.Placeholder = (string)e.NewValue;
        }



        public int Quantidade { get; private set; }



        public InputNumber()
        {
            InitializeComponent();

            Loaded += delegate
            {
                Quantidade = 0;
                RefreshValue();
            };
        }



        public void Zero(bool disable = false)
        {
            Text = "0";
            Quantidade = 0;

            if (disable)
                IsEnabled = false;
        }



        private void RefreshValue()
        {
            Text = Quantidade.ToString();
        }



        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
                return;
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox input = (TextBox)sender;
            string text = input.Text;

            if (Str.IsNumber(text))
                Quantidade = Convert.ToInt32(text);
        }



        private void Up_Click(object sender, RoutedEventArgs e)
        {
            Quantidade += 1;
            RefreshValue();
        }



        private void Down_Click(object sender, RoutedEventArgs e)
        {
            Quantidade -= 1;
            RefreshValue();
        }
    }
}
