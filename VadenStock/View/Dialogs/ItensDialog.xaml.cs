using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ItensDialog : Border
    {
        public ItensDialog()
        {
            InitializeComponent();

            Loaded += delegate
            {
                Row row = new Row()
                    .TD("0523")
                    .TD("PowerBeam M5")
                    .TD("04:18:D6:E6:CE:61")
                    .TD("Em Rota");

                _TableItens.Headers("Código", "Produto", "MAC", "Status");
                _TableItens.Add(row);
                _TableItens.Draw();
            };
        }



        public ItensDialog(List<ProdutoType> dataset)
        {
            InitializeComponent();

            Loaded += delegate
            {
                _TableItens.Headers("Produto", "Marca", "Categoria", "Preço");

                foreach (ProdutoType pt in dataset)
                    _TableItens.Add(
                                new Row(new Row.Options()
                                {
                                    Hover = true,
                                    Cursor = Cursors.Hand
                                })
                                .TD(pt.Name)
                                .TD(pt.Marca.Name)
                                .TD(pt.Categoria.Name)
                                .TD(Str.Currency((pt.Price * 100).ToString()))
                        );

                _TableItens.Draw();
            };
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
