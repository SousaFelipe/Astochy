using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Containers;



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



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
