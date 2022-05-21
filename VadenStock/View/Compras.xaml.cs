using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Adapters;
using VadenStock.View.Models;



namespace VadenStock.View
{
    public partial class Compras : UserControl
    {
        public Compras()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadOrcamentos();
            };
        }



        private void LoadOrcamentos()
        {
            List<CompraType> compras = ComprasViewModel.ComprasPorStatus(CompraType.CompraStatus.Orcamento);
            OrcamentosAdapter adapter = new(_GridOrcamentos);

            if (compras.Count > 1)
            {
                _StackOrcamentos.Visibility = Visibility.Visible;
                adapter.Update(compras);
            }
            else
            {
                _StackOrcamentos.Visibility = Visibility.Collapsed;
                adapter.Clear();
            }
        }
    }
}
