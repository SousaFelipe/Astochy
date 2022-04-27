using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components
{
    public partial class Dropdown : ComboBox
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(Dropdown),
                new UIPropertyMetadata(string.Empty, LabelPropertyCallback)
            );



        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }



        public static void LabelPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Dropdown dropdown = (Dropdown)root;
            dropdown.Label = (string)e.NewValue;
        }



        public Dropdown()
        {
            InitializeComponent();
        }
    }
}
