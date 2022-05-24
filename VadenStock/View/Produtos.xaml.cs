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
                LoadMarcas();
                LoadProdutos();
            };
        }



        private bool LoadCategorias()
        {
            if (_ComboCategorias != null)
            {
                if (_ComboCategorias.Items.Count > 1)
                    _ComboCategorias.Clear(true);

                foreach (CategoriaType c in CategoriasViewModel.TodasAsCategorias)
                {
                    _ComboCategorias.Items.Add(new ComboBoxItem()
                    {
                        Tag = c.Id.ToString(),
                        Content = c.Name
                    });
                }
            }

            return true;
        }



        private bool LoadTipos()
        {
            if (_ComboTipos != null)
            {
                if (_ComboTipos.Items.Count > 1)
                    _ComboTipos.Clear(true);

                foreach (TipoType t in TiposViewModel.TiposPorCategoria(Filter.Categoria))
                {
                    _ComboTipos.Items.Add(new ComboBoxItem()
                    {
                        Tag = t.Id.ToString(),
                        Content = t.Name
                    });
                }
            }

            return true;
        }



        private bool LoadMarcas()
        {
            if (_ComboMarcas.Items.Count > 1)
                _ComboMarcas.Clear(true);

            foreach (MarcaType m in MarcasViewModel.TodasAsMarcas)
            {
                _ComboMarcas.Items.Add(new ComboBoxItem()
                {
                    Tag = m.Id.ToString(),
                    Content = m.Name
                });
            }

            return true;
        }



        public bool LoadProdutos()
        {
            List<ProdutoType> produtos = ProdutosViewModel.FilterData(Filter);

            ProdutosAdapter adapter = new(_GridProdutos);
            adapter.SetView(this);
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
                        window.DisplayDialog(new MarcaDialog(), LoadMarcas);
                        break;

                    case "C":
                        window.DisplayDialog(new CategoriaDialog(), LoadCategorias);
                        break;

                    case "T":
                        window.DisplayDialog(new TipoDialog(), LoadTipos);
                        break;
                }

                cb.SelectedItem = null;
            }
        }



        private void SelectCategorias_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            if (item != null)
            {
                Filter.Categoria = Convert.ToInt32(item.Tag);
                LoadTipos();
                LoadProdutos();
            }
        }



        private void SelectTipos_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            if (item != null)
            {
                Filter.Tipo = Convert.ToInt32(item.Tag);
                LoadProdutos();
            }
        }



        private void SelectMarcas_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox box = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;

            if (item != null)
            {
                Filter.Marca = Convert.ToInt32(item.Tag);
                LoadProdutos();
            }
        }
    }
}
