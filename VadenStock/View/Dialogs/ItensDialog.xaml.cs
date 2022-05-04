using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        }



        public ItensDialog(List<ProdutoType> dataset)
        {
            InitializeComponent();

            Loaded += delegate
            {
                _TableItens.DefaultOptions.Stripped = true;

                _TableItens.Headers(
                        Header.Auto("Produto"),
                        Header.Max("Marca"),
                        Header.Max("Categoria"),
                        Header.Max("Tipo"),
                        Header.Max("Preço")
                    );

                foreach (ProdutoType pt in dataset)
                {
                    _TableItens.Add(
                            new Row()
                                .TD(pt.Name)
                                .TD(pt.Marca.Name)
                                .TD(pt.Categoria.Name)
                                .TD(pt.Tipo.Name)
                                .TD(Str.Currency((pt.Price * 100).ToString()))
                        );
                }

                _Pagination.Table = _TableItens;
                _Pagination.Update();
            };
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
