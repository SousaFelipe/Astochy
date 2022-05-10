using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.View.Models;
using VadenStock.View.Adapters;
using VadenStock.View.Dialogs;

using VadenStock.Model.Types;
using VadenStock.View.Structs;
using VadenStock.View.Components.Forms;



namespace VadenStock.View
{
    public partial class Produtos : UserControl
    {
        private ProdutoStruct Filter = new();



        public Produtos()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadCategorias();
                LoadProdutos();
            };
        }



        private void LoadCategorias()
        {
            foreach (CategoriaType c in CategoriasViewModel.TodasAsCategorias)
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = c.Id.ToString(),
                    Content = c.Name
                });
            }
        }



        public bool LoadProdutos()
        {
            ProdutosAdapter adapter = new(_GridProdutos);
            List<ProdutoType> produtos = ProdutosViewModel.FilterData(Filter);

            adapter.Clear();
            adapter.Update(produtos);

            return true;
        }



        private void ComboCadastroSelectedOption(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedItem is ComboBoxItem cbi)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                switch (cbi.Tag)
                {
                    case "P":
                        window.DisplayDialog(new ProdutoDialog(), LoadProdutos);
                        break;

                    case "M":
                        break;

                    case "C":
                        break;

                    case "T":
                        break;
                }

                cb.SelectedItem = null;
            }
        }



        private void SelectCategorias_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            Filter.Categoria = Convert.ToInt32(item.Tag);

            LoadProdutos();
        }



        private void SelectTipos_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            Filter.Tipo = Convert.ToInt32(item.Tag);

            LoadProdutos();
        }



        private void SelectMarcas_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            Filter.Marca = Convert.ToInt32(item.Tag);

            LoadProdutos();
        }
    }
}
