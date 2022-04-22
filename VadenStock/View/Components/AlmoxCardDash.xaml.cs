using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace VadenStock.View.Components
{
    public partial class AlmoxCardDash : UserControl
    {
        public static readonly DependencyProperty CardColorProp = DependencyProperty.Register(
                "CardColor",
                typeof(string),
                typeof(AlmoxCardDash),
                new UIPropertyMetadata(string.Empty, CardColorCallback)
            );

        public string CardColor
        {
            get { return (string)GetValue(CardColorProp); }
            set { SetValue(CardColorProp, value); }
        }

        public static void CardColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            AlmoxCardDash card = (AlmoxCardDash)root;
            card._BorderBody.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)e.NewValue));
        }



        public AlmoxCardDash()
        {
            InitializeComponent();
        }
    }
}
