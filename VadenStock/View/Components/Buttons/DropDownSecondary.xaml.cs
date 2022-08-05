using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components.Buttons
{
    public partial class DropDownSecondary : ComboBox
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(DropDownSecondary),
                new UIPropertyMetadata(string.Empty, LabelPropertyCallback)
            );



        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }



        public static void LabelPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            DropDownSecondary dropdown = (DropDownSecondary)root;
            dropdown.Label = (string)e.NewValue;
        }



        public DropDownSecondary()
        {
            InitializeComponent();
        }
    }
}
