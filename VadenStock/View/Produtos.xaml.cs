using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

using VadenStock.View.Models;
using VadenStock.View.Adapters;
using VadenStock.View.Dialogs;

using VadenStock.Model.Types;
using VadenStock.View.Structs;
using VadenStock.View.Components;


namespace VadenStock.View
{
    public partial class Produtos : UserControl
    {
        private ProdutoFilter Filter = new();



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
            foreach (CategoriaType c in CategoriasViewModel.GetCategorias())
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = c.Id.ToString(),
                    Content = c.Name
                });
            }
        }



        public void LoadProdutos()
        {
            ProdutosAdapter adapter = new(_GridProdutos);

            adapter.Clear();
            adapter.Update(ProdutosViewModel.GetProdutos(Filter), false);
            adapter.Build();
        }



        private void ComboCadastroSelectedOption(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedItem is ComboBoxItem cbi)
            {
                VadenStock.MainWindow window = (VadenStock.MainWindow)Application.Current.MainWindow;
                window.EnterDialogMode();

                switch (cbi.Tag)
                {
                    case "P":
                        ProdutoDialog dialog = new();
                        dialog.ShowDialog();
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
