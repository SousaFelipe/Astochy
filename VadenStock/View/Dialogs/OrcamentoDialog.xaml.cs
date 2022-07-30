using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VadenStock.Model;
using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Forms;
using VadenStock.View.Models;



namespace VadenStock.View.Dialogs
{
    public partial class OrcamentoDialog : Border
    {
        private CompraType Orcamento;
        private ItemCompraType CurrentItem;
        private List<ItemCompraType> Itens;



        public OrcamentoDialog()
        {
            Orcamento = new()
            {
                Fornecedor = new FornecedorType() { Id = 0 },
                Status = CompraType.CompraStatus.Orcamento,
                ValorTotal = 0
            };

            CurrentItem = new()
            {
                Compra = new() { Id = 0 },
                Produto = new() { Id = 0 },
                Quantidade = 0,
                ValorTotal = 0
            };

            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadFornecedores();
                LoadMarcas();
            };
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("N°"),
                        Header.Max("RODUTO"),
                        Header.Auto("VAL UNI."),
                        Header.Auto("QTD."),
                        Header.Auto("VAL TOT."),
                        Header.Auto("Ação")
                    );

            _PaginationItens.Table = _TableItens;
        }



        private void LoadFornecedores()
        {
            _SelectFornecedores.Clear(true);

            foreach (FornecedorType fornecedor in FornecedoresViewModel.TodosOsFornecedores)
            {
                _SelectFornecedores.Items.Add(new ComboBoxItem()
                {
                    Tag = fornecedor.Id,
                    Content = fornecedor.Tag
                });
            }
        }



        private void LoadMarcas()
        {
            _SelectMarcas.Clear(true);

            foreach (MarcaType m in MarcasViewModel.TodasAsMarcas)
            {
                _SelectMarcas.Items.Add(new ComboBoxItem()
                {
                    Tag = m.Id,
                    Content = m.Name
                });
            }
        }



        private void LoadProdutos(int marca)
        {
            List<ProdutoType> produtos = ProdutosViewModel.ProdutosPorMarca(marca);

            _SelectProdutos.Clear(true);

            if (produtos != null && produtos.Count > 0)
            {
                foreach (ProdutoType produto in produtos)
                {
                    _SelectProdutos.Items.Add(new ComboBoxItem()
                    {
                        Tag = produto.Id,
                        Content = produto.Name
                    });
                }
            }
        }



        private void ShouldBeSavedEnabled()
        {
            if (_ButtonSave != null)
            {
                _ButtonSave.IsEnabled = (
                    Orcamento.Fornecedor.Id > 0 &&
                    Orcamento.ValorTotal > 0 &&
                    Itens.Count > 0
                );
            }
        }



        private void RefreshTable()
        {
            _TableItens.Clear();
            _PaginationItens.Clear();
            _ButtonSave.IsEnabled = (Itens.Count > 0);

            if (Itens.Count > 0)
            {
                int num = 1;

                _StackEmpty.Visibility = Visibility.Collapsed;

                foreach (ItemCompraType item in Itens)
                {
                    _TableItens.Add(
                            new Row()
                                .TD(Str.ZeroFill(num))
                                .TD(item.Produto.Name)
                                .TD(Str.Currency(Convert.ToString(item.Produto.Price * 100)))
                                .TD(Str.ZeroFill(item.Quantidade))
                                .TD(Str.Currency(Convert.ToString((item.Produto.Price * 100) * item.Quantidade)))
                                .AC("X", Row.ActionLevel.Danger, sender =>
                                {

                                })
                        );

                    num++;
                }

                _PaginationItens.Paginate();
            }
            else
            {
                _StackEmpty.Visibility = Visibility.Visible;
            }
        }



        private void RefreshTotal()
        {
            Orcamento.ValorTotal = 0;

            foreach (ItemCompraType item in Itens)
                Orcamento.ValorTotal += item.ValorTotal;
        }



        private void SelectFornecedores_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    FornecedorType? fornecedor = FornecedoresViewModel.Find(tag);
                    if (fornecedor != null)
                        Orcamento.Fornecedor = fornecedor.Value;
                }
            }

            ShouldBeSavedEnabled();
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

                    if (marca != null)
                        LoadProdutos(marca.Value.Id);

                    else if (_SelectProdutos != null)
                        _SelectProdutos.Clear(true);
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
                        CurrentItem.Produto = produto.Value;
                }
            }

            ShouldBeSavedEnabled();
        }



        private void InputQuantidade_Changed(object sender, TextChangedEventArgs e)
        {
            InputNumber input = (InputNumber)sender;

            if (input.Quantidade >= 0)
            {
                CurrentItem.Quantidade = input.Quantidade;
                CurrentItem.ValorTotal = CurrentItem.Quantidade * (CurrentItem.Produto.Price);
            }

            RefreshTotal();
            ShouldBeSavedEnabled();
        }



        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (Orcamento.Fornecedor.Id <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de selecionar o fornecedor"));

            else if (CurrentItem.Quantidade <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de informar a quantidade de produtos"));

            else if (CurrentItem.Produto.Id <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de selecionar o produto"));

            else
            {
                Itens.Add(CurrentItem);

                CurrentItem = new()
                {
                    Compra = new() { Id = 0 },
                    Produto = new() { Id = 0 },
                    Quantidade = 0,
                    ValorTotal = 0
                };

                _InputQuantidade.Zero();

                LoadMarcas();
                LoadProdutos(0);
                RefreshTable();
                RefreshTotal();
            }

            ShouldBeSavedEnabled();
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (Orcamento.Fornecedor.Id <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de selecionar o fornecedor"));

            else if (Itens == null || Itens.Count <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa adicionar ao menos um item"));

            else if (ComprasViewModel.Create(Orcamento))
            {
                ItemCompraType item;
                CompraType last = ComprasViewModel.Last() ?? new CompraType() { Id = 0 };

                int save;
                for (save = 0; save < Itens.Count; save++)
                {
                    item = Itens[save];
                    item.Compra = last;

                    if (!ItensComprasViewModel.Create(item))
                    {
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, $"Erro ao salvar o item \"{item.Produto.Name}\""));
                        break;
                    }
                }

                if (save == Itens.Count)
                {
                    LoadFornecedores();
                    LoadMarcas();
                    LoadProdutos(0);

                    Itens.Clear();

                    RefreshTable();
                    RefreshTotal();

                    _InputQuantidade.Zero();

                    Orcamento = new()
                    {
                        Fornecedor = new FornecedorType() { Id = 0 },
                        Status = CompraType.CompraStatus.Orcamento,
                        ValorTotal = 0
                    };

                    CurrentItem = new()
                    {
                        Compra = new() { Id = 0 },
                        Produto = new() { Id = 0 },
                        Quantidade = 0,
                        ValorTotal = 0
                    };

                    ShouldBeSavedEnabled();

                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Orçamento realizado com sucesso"));
                }
            }
            else
            {
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao salvar o orçamento"));
            }
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
