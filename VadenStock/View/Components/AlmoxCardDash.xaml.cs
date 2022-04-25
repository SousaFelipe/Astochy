using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using VadenStock.Model.Types;



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



        public AlmoxType Contract { get; private set; }



        public AlmoxCardDash(AlmoxType contract)
        {
            Contract = contract;

            InitializeComponent();

            Loaded += delegate
            {
                OnBeforeLoaded();
            };
        }



        private void OnBeforeLoaded()
        {
            string iconName = "warehouse";

            switch (Contract.Tipo)
            {
                case AlmoxType.Hosted.Carro:
                    iconName = "car";
                    break;

                case AlmoxType.Hosted.Moto:
                    iconName = "bike";
                    break;
            }

            _TextName.Text = Contract.Name;
            _ImageTipo.Source = new BitmapImage(new Uri($"/VadenStock;component/Resources/Icons/{ iconName }.png", UriKind.Relative));
        }



        public void SetItens(List<ItemType> itens)
        {
            if (itens != null)
            {
                int qtd = itens.Count;
                string qtdStr = (qtd > 0 && qtd < 10) ? $"0{ qtd }" : $"{ qtd }";

                _TextQuantidate.Text = $"{ qtdStr } equipamentos";
            }
        }
    }
}
