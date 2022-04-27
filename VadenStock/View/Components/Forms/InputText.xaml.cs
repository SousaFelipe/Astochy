using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components.Forms
{
    public partial class InputText : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
                "Placeholder",
                typeof(string),
                typeof(InputText),
                new UIPropertyMetadata(string.Empty, PlaceholderPropertyCallback)
            );



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }



        public static void PlaceholderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            InputText instance = (InputText)root;
            instance.Placeholder = (string)e.NewValue;
        }



        public InputText()
        {
            InitializeComponent();
        }
    }
}
