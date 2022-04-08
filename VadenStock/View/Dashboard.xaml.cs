using System;
using System.Windows.Controls;

using VadenStock.View.Models;



namespace VadenStock.View
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();

            LoadCardEstoqueMinMax();
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
        }



        private void ComboCategorias_Update(object sender, SelectionChangedEventArgs e)
        {
            object tag = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            int tagNum = int.Parse(tag.ToString());

            if (tagNum > 0)
            {
                var tipos = DashboardViewModel.GetTipos(tagNum);

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
