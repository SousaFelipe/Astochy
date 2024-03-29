﻿using System.Windows;
using System.Windows.Controls;

using VadenStock.Tools;



namespace VadenStock.View.Components
{
    public partial class Patrimonio : UserControl
    {
        public static readonly DependencyProperty QuantidateProp = DependencyProperty.Register(
                "Quantidate",
                typeof(string),
                typeof(Patrimonio),
                new UIPropertyMetadata(string.Empty, QuantidadeCallback)
            );

        public static readonly DependencyProperty TituloProp = DependencyProperty.Register(
                "Titulo",
                typeof(string),
                typeof(Patrimonio),
                new UIPropertyMetadata(string.Empty, TituloCallback)
            );



        public string Quantidade
        {
            get { return (string)GetValue(QuantidateProp); }
            set { SetValue(QuantidateProp, value); }
        }

        public string Titulo
        {
            get { return (string)GetValue(TituloProp); }
            set { SetValue(TituloProp, value); }
        }



        private static void QuantidadeCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Patrimonio patrimonio = (Patrimonio)root;
            patrimonio._TextBlockQuantidade.Text = (string)e.NewValue;
        }

        private static void TituloCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Patrimonio patrimonio = (Patrimonio)root;
            patrimonio._TextBlockTitulo.Text = (string)e.NewValue;
        }



        public Patrimonio()
        {
            InitializeComponent();
        }



        public void UpdateValue(double valor, double total)
        {
            double percent = valor <= 0 ? 0 : valor * 100 / total;

            _TextBlockPercent.Text = string.Concat(percent.ToString("C2").Replace("R$ ", ""), "%");

            _Chart.SetSeries(new double[] { valor });
            _Chart.Draw(percent, 10);

            Quantidade = valor.ToString();
        }
    }
}
