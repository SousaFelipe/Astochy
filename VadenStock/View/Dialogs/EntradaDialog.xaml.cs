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



        public EntradaDialog()
        {
            InventarioType? inventario = InventariosViewModel.Find(1);

            NovoItem = new()
            {
                Inventario = inventario != null
                    ? (InventarioType)inventario
                    : new InventarioType() { Id = 1 },
            };

            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadMarcas();
                LoadAlmoxarifados();
            };
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("Cod."),
                        Header.Auto("MAC"),
                        Header.Max("Produto"),
                        Header.Auto("Ação")
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
                                .AC("X", Row.ActionLevel.Danger, delegate
                                {
                                    return RemoveItemFromEntrada(item.Codigo);
                                })
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

            foreach (MarcaType m in MarcasViewModel.TodasAsMarcas)
            {
                _ComboMarcas.Items.Add(new ComboBoxItem()
                {
                    Tag = m.Id,
                    Content = m.Name
                });
            }
        }



        private void LoadProdutos(int marca)
        {
            _ComboProdutos.Clear(true);

            foreach (ProdutoType p in ProdutosViewModel.ProdutosPorMarca(marca))
            {
                _ComboProdutos.Items.Add(new ComboBoxItem()
                {
                    Tag = p.Id,
                    Content = p.Name
                });
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



        private void AddNovoItem()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            foreach (ItemType i in Itens)
            {
                if (i.Codigo == NovoItem.Codigo || i.Mac == NovoItem.Mac)
                {
                    window.DisplayAlert(
                        new AlertDialog(AlertDialog.AlertType.Info, "Este item já foi adicionado à lista")
                        );

                    goto exit_function;
                }
            }

            Itens.Add(NovoItem);
            RefreshTable();

            _ComboMarcas.SelectedIndex = 0;
            _ComboAlmoxarifados.SelectedIndex = 0;

            _ComboProdutos.Clear(true);
            _InputMAC.Clear();
            _InputCodigo.Clear();

            NovoItem.Codigo = string.Empty;
            NovoItem.Mac = string.Empty;

        exit_function:
            ;
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



        private void ComboMarcas_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    MarcaType? marca = MarcasViewModel.Find(tag);
                    if (marca != null)
                        LoadProdutos(tag);
                }
            }
        }



        private void ComboProdutos_Changed(object sender, SelectionChangedEventArgs e)
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



        private void ComboAlmoxarifados_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    AlmoxType? almox = AlmoxarifadosViewModel.Find(tag);

                    if (almox != null)
                    {
                        NovoItem.Almoxarifado = (AlmoxType)almox;
                        NovoItem.Localizado = NovoItem.Almoxarifado.Acao;
                    }
                }
            }
        }



        private void InputMAC_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;

            if (input.Text.Length <= 12)
            {
                string mac = input.Text;

                if (!string.IsNullOrEmpty(mac) && mac.Length >= 12)
                {
                    ItemType? item = ItensViewModel.Find(mac);

                    if (item == null)
                        NovoItem.Mac = mac;

                    else
                    {
                        MainWindow window = (MainWindow)Application.Current.MainWindow;
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Este equipamento já está registrado"));
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

            else if (string.IsNullOrEmpty(_InputCodigo.Text))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor, insira o Código"));

            else
            {
                string codigo = _InputCodigo.Text;
                ItemType? item = ItensViewModel.Find(codigo);

                if (item == null)
                {
                    NovoItem.Codigo = codigo;
                    AddNovoItem();
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Este equipamento já está registrado"));
            }
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int saveCount = 0;

            if (Itens != null && Itens.Count > 0)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                foreach (ItemType it in Itens)
                {
                    if (ItensViewModel.Create(it) > 0)
                        saveCount++;

                    else
                    {
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, $"Ocorreu um erro ao salvar o item '{it.Codigo}'"));
                        break;
                    }
                }

                if (saveCount == Itens.Count)
                {
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, $"Entrada salva com sucesso!"));

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
