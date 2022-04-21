using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.View.Models;
using VadenStock.View.Structs;
using VadenStock.View.Components.Charts;



namespace VadenStock.View
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();

            Loaded += delegate {
                LoadCardPatrimonio();
                LoadCardEstoqueMinMax();
            };
        }



        public void LoadCardPatrimonio()
        {
            /*PatrimonioS patrimonio = DashboardViewModel.GetPatrimonio();

            _PatrimonioEmEstoque.UpdateValue(patrimonio.Estoque, patrimonio.Total);
            _PatrimonioEmComodato.UpdateValue(patrimonio.Comodato, patrimonio.Total);
            _PatrimonioEmProducao.UpdateValue(patrimonio.Producao, patrimonio.Total);*/

            _PatrimonioEmEstoque.UpdateValue(488, 3452);
            _PatrimonioEmComodato.UpdateValue(1823, 3452);
            _PatrimonioEmProducao.UpdateValue(252, 3452);
        }



        private void LoadCardEstoqueMinMax()
        {
            var categorias = DashboardViewModel.Categorias;

            for (int i = 0; i < categorias.Count; i++)
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = categorias[i].Id,
                    Content = string.Concat(categorias[i].Name[0], categorias[i].Name[1..].ToLower())
                });
            }

            _ColumnChartMinMax.SetSeries(new double[] { 45, 23, 84, 19, 61, 42, 77 });
            _ColumnChartMinMax.SetLabels(new string[] { "MKT", "UBQ", "ITB", "HWY", "SAE", "OPB", "LNA" });
            _ColumnChartMinMax.Draw();
        }



        private void ComboCategorias_Update(object sender, SelectionChangedEventArgs e)
        {
            object tag = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            int tagNum = int.Parse(tag.ToString());

            if (_ComboTipos != null)
            {
                var tipos = DashboardViewModel.GetTipos(tagNum);

                _ComboTipos.Clear(true);

                for (int i = 0; i < tipos.Count; i++)
                {
                    _ComboTipos.Items.Add(new ComboBoxItem()
                    {
                        Tag = tipos[i].Id,
                        Content = string.Concat(tipos[i].Name[0], tipos[i].Name[1..].ToLower())
                    });
                }
            }
        }
    }
}
