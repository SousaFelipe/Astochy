using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class EntradaDialog : Border
    {
        private ItemType NovoItem;
        private readonly List<ItemType> Itens;


        private readonly ProdutoType Filtro;



        public EntradaDialog()
        {
            Filtro = new()
            {
                Id = 0,
                Categoria = new() { Id = 0 },
                Tipo = new() { Id = 0 },
                Marca = new() { Id = 0 },
                Name = string.Empty,
                Description = string.Empty
            };

            NovoItem = new();
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadMarcas();
                LoadCategorias();
                LoadAlmoxarifados();
            };
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("CÓD."),
                        Header.Auto("MAC"),
                        Header.Max("PRODUTO"),
                        Header.Auto("AÇÃO")
                    );

            _PaginationItens.Table = _TableItens;
        }



        private void RefreshTable()
        {
            _TableItens.Clear();
            _PaginationItens.Clear();
            _ButtonSave.IsEnabled = (Itens.Count > 0);

            if (Itens.Count > 0)
            {
                _StackEmpty.Visibility = Visibility.Collapsed;

                foreach (ItemType item in Itens)
                {
                    _TableItens.Add(
                            new Row()
                                .TD(item.Codigo)
                                .TD(Str.MAC(item.Mac))
                                .TD(item.Produto.Name)
                                .AC("X", Row.ActionLevel.Danger, sender => RemoveItemFromEntrada(item.Codigo))
                        );
                }

                _PaginationItens.Paginate();
            }
            else
            {
                _StackEmpty.Visibility = Visibility.Visible;
            }
        }



        private void LoadMarcas()
        {
            _ComboMarcas.Clear(true);

            foreach (MarcaType marca in MarcasViewModel.TodasAsMarcas)
            {
                _ComboMarcas.Items.Add(new ComboBoxItem()
                {
                    Tag = marca.Id,
                    Content = marca.Name
                });
            }
        }



        private void LoadCategorias()
        {
            _SelectCategorias.Clear(true);

            foreach (CategoriaType categoria in CategoriasViewModel.TodasAsCategorias)
            {
                _SelectCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = categoria.Id,
                    Content = categoria.Name
                });
            }
        }



        private void LoadProdutos()
        {
            _ComboProdutos.Clear(true);

            if (Filtro.Marca.Id > 0 && Filtro.Categoria.Id > 0)
            {
                List<ProdutoType> produtos = ProdutosViewModel.Read(
                    new object[] { "marca", Filtro.Marca.Id },
                    new object[] { "categoria", Filtro.Categoria.Id }
                );

                foreach (ProdutoType produto in produtos)
                {
                    _ComboProdutos.Items.Add(new ComboBoxItem()
                    {
                        Tag = produto.Id,
                        Content = produto.Name
                    });
                }
            }
        }



        private void LoadAlmoxarifados()
        {
            _ComboAlmoxarifados.Clear(true);

            foreach (AlmoxType a in AlmoxarifadosViewModel.TodosOsAlmoxarifados)
            {
                _ComboAlmoxarifados.Items.Add(new ComboBoxItem()
                {
                    Tag = a.Id,
                    Content = a.Name
                });
            }

            _ComboAlmoxarifados.SelectedIndex = 0;
        }



        private bool RemoveItemFromEntrada(string codigo)
        {
            foreach (ItemType item in Itens)
            {
                if (item.Codigo == codigo)
                {
                    Itens.Remove(item);
                    RefreshTable();
                    return true;
                }
            }

            return false;
        }



        private void SelectMarcas_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    MarcaType? marca = MarcasViewModel.Find(tag);
                    Filtro.Marca = (marca != null) ? marca.Value : new() { Id = 0 };
                    LoadProdutos();
                }
            }
        }



        private void SelectCategorias_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    List<CategoriaType> categorias = CategoriasViewModel.Read(new object[] { "id", tag });
                    Filtro.Categoria = (categorias.Count > 0) ? categorias[0] : new CategoriaType() { Id = 0 };
                    LoadProdutos();
                }
            }
        }



        private void SelectProdutos_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    ProdutoType? produto = ProdutosViewModel.Find(tag);

                    if (produto != null)
                        NovoItem.Produto = (ProdutoType)produto;
                }
            }
        }



        private void SelectAlmoxarifados_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    AlmoxType? almox = AlmoxarifadosViewModel.Find(new object[] { "id", tag });

                    if (almox != null)
                    {
                        NovoItem.Almoxarifado = (AlmoxType)almox;
                        NovoItem.Localizado = NovoItem.Almoxarifado.Acao;
                    }
                }
            }
        }



        private void InputCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string codigo = input.Text.Trim();

            if (!string.IsNullOrEmpty(codigo))
            {
                ItemType? item = ItensViewModel.Find(codigo);

                if (item == null)
                    NovoItem.Codigo = codigo;

                else
                {
                    NovoItem.Codigo = string.Empty;
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, $"Já existe um equipamento registrado com o código \"{ codigo }\""));
                }
            }
        }



        private void InputMAC_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string mac = input.Text.Trim().Replace(":", "");

            if (mac.Length <= 12)
            {
                if (!string.IsNullOrEmpty(mac) && mac.Length == 12)
                {
                    ItemType? item = ItensViewModel.Find(mac);

                    if (item == null)
                        NovoItem.Mac = mac;

                    else
                    {
                        MainWindow window = (MainWindow)Application.Current.MainWindow;
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, $"Já existe um equipamento registrado com o MAC \"{ mac }\""));
                    }
                }
            }
            else
            {
                input.Text = input.Text.Remove(input.Text.Length - 1);
                input.CaretIndex = input.Text.Length;
            }
        }



        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (_ComboMarcas.SelectedIndex <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor, selecione a Marca"));

            else if (_ComboProdutos.SelectedIndex <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor, selecione o Produto"));

            else if (_ComboAlmoxarifados.SelectedIndex <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor, selecione o Almoxarifado"));

            else if (string.IsNullOrEmpty(NovoItem.Codigo))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor, insira o Código"));

            else if (!string.IsNullOrEmpty(NovoItem.Mac) && NovoItem.Mac.Length != 12)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "O MAC inserido está no formato incorreto"));

            else
            {
                foreach (ItemType i in Itens)
                {
                    if (i.Codigo == NovoItem.Codigo || i.Mac == NovoItem.Mac)
                    {
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Info, "Este item já foi adicionado à lista"));
                        goto exit_function;
                    }
                }

                Itens.Add(NovoItem);
                RefreshTable();

                _InputMAC.Clear();
                _InputCodigo.Clear();

                NovoItem.Codigo = string.Empty;
                NovoItem.Mac = string.Empty;

                _InputCodigo.Focus();
            }

        exit_function:
            ;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int saveCount = 0;

            if (Itens != null && Itens.Count > 0)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                foreach (ItemType it in Itens)
                {
                    if (ItensViewModel.Create(it))
                        saveCount++;

                    else
                    {
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, $"Ocorreu um erro ao salvar o item '{it.Codigo}'"));
                        break;
                    }
                }

                if (saveCount == Itens.Count)
                {
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, $"Entrada salva com sucesso"));

                    Itens.Clear();
                    RefreshTable();
                }
            }
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
