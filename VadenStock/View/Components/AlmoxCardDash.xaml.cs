using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using VadenStock.Model;



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

        public static readonly DependencyProperty TipoProp = DependencyProperty.Register(
                "Tipo",
                typeof(object),
                typeof(AlmoxCardDash),
                new UIPropertyMetadata(string.Empty, TipoCallback)
            );


        public string CardColor
        {
            get { return (string)GetValue(CardColorProp); }
            set { SetValue(CardColorProp, value); }
        }

        public object Tipo
        {
            get { return (Almoxarifado.Contract.ETipo)GetValue(TipoProp); }
            set { SetValue(TipoProp, value); }
        }



        public static void CardColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            AlmoxCardDash card = (AlmoxCardDash)root;
            card._BorderBody.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)e.NewValue));
        }

        public static void TipoCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            AlmoxCardDash card = (AlmoxCardDash)root;
            Almoxarifado.Contract.ETipo tipo = (Almoxarifado.Contract.ETipo)e.NewValue;

            string iconName = (tipo == Almoxarifado.Contract.ETipo.Carro)
                    ? "car"
                    : (tipo == Almoxarifado.Contract.ETipo.Moto)
                        ? "bike"
                        : "warehouse";

            card._ImageTipo.Source = new BitmapImage(
                    new Uri($"/VadenStock;component/Resources/Icons/{ iconName }.png", UriKind.Relative)
                );
        }



        private Almoxarifado.Contract contract;
        private List<Item.Contract> Items = new();



        public AlmoxCardDash(Almoxarifado.Contract almoxarifado)
        {
            contract = almoxarifado;

            InitializeComponent();

            Loaded += delegate
            {
                _TextName.Text = contract.Name;
            };
        }



        public void SetItens(List<Item.Contract> itens)
        {
            if (itens != null)
            {
                int qtd = itens.Count;
                string qtdStr = (qtd > 0 && qtd < 10) ? $"0{ qtd }" : $"{ qtd }";

                _TextQuantidate.Text = $"{ qtdStr } equipamentos";

                Items = itens;
            }
        }
    }
}
