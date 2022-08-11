using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Widgets;
using VadenStock.View.Models;



namespace VadenStock.View.Dialogs
{
    public partial class OrcamentoDialog : Border
    {
        private CompraType NovoOrcamento;
        private readonly CompraType? EditarOrcamento;

        private ItemCompraType CurrentItem;
        private List<ItemCompraType> Itens;



        public OrcamentoDialog()
        {
            EditarOrcamento = null;
            NovoOrcamento = new()
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



        public OrcamentoDialog(CompraType compra)
        {
            EditarOrcamento = compra;
            NovoOrcamento = new() { Id = 0 };

            CurrentItem = new()
            {
                Compra = EditarOrcamento,
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

                RefreshControls();
                SelectFornecedor(compra.Fornecedor);
                RefreshItens();

                if (EditarOrcamento.Status == CompraType.CompraStatus.Recebida)
                    DisableControls();
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
                        Header.Auto("AÇÃO")
                    );

            _PaginationItens.Table = _TableItens;
        }



        private void LoadFornecedores()
        {
            _SelectFornecedores.Clear(true);

            foreach (FornecedorType f in FornecedoresViewModel.TodosOsFornecedores)
            {
                _SelectFornecedores.Items.Add(new ComboBoxItem()
                {
                    Tag = f.Id,
                    Content = f.Tag
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
                    (EditarOrcamento ?? NovoOrcamento).Fornecedor.Id > 0 &&
                    (EditarOrcamento ?? NovoOrcamento).ValorTotal > 0 &&
                    (EditarOrcamento ?? NovoOrcamento).Status != CompraType.CompraStatus.Aprovada &&
                    (EditarOrcamento ?? NovoOrcamento).Status != CompraType.CompraStatus.Recebida &&
                    Itens.Count > 0
                );
            }

            if (_SelectFornecedores != null)
                _SelectFornecedores.IsEnabled = Itens.Count <= 0;
        }



        private void DisableControls()
        {
            _SelectFornecedores.IsEnabled = false;
            _SelectMarcas.IsEnabled = false;
            _SelectProdutos.IsEnabled = false;
            _InputQuantidade.IsEnabled = false;
            _ButtonEntrada.IsEnabled = false;
            _ButtonSave.IsEnabled = false;
        }



        private void RefreshControls()
        {
            if (EditarOrcamento != null)
            {
                if (EditarOrcamento.Status == CompraType.CompraStatus.Cancelada || EditarOrcamento.Status == CompraType.CompraStatus.Recebida)
                {
                    _StackControls.Visibility = Visibility.Collapsed;
                    goto exit_function;
                }

                _StackControls.Visibility = Visibility.Visible;
                _ButtonCancelar.Visibility = EditarOrcamento.Status != CompraType.CompraStatus.Recebida ? Visibility.Visible : Visibility.Collapsed;
                _ButtonAprovar.Visibility = EditarOrcamento.Status == CompraType.CompraStatus.Orcamento ? Visibility.Visible : Visibility.Collapsed;
            }

        exit_function:
            ;
        }



        private void SelectFornecedor(FornecedorType? fornecedor)
        {
            if (EditarOrcamento != null && fornecedor != null)
            {
                foreach (ComboBoxItem item in _SelectFornecedores.Items)
                {
                    if (Convert.ToInt32(item.Tag) == fornecedor.Value.Id)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
        }



        private void RefreshItens()
        {
            if (EditarOrcamento != null)
            {
                Itens = ItensComprasViewModel.Read(new object[] { "compra", EditarOrcamento.Id });

                int savedItens = 0;
                foreach (ItemCompraType itc in Itens)
                    if (itc.Status == ItemCompraType.ICStatus.Baixado)
                        savedItens++;

                if (savedItens == Itens.Count)
                {
                    if (EditarOrcamento.Status != CompraType.CompraStatus.Recebida)
                    {
                        EditarOrcamento.Status = CompraType.CompraStatus.Recebida;

                        if (ComprasViewModel.Update(EditarOrcamento) > 0)
                        {
                            MainWindow window = (MainWindow)Application.Current.MainWindow;
                            window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Todos os itens da compra foram recebidos"));
                        }
                    }

                    DisableControls();
                }

                RefreshTable();
                RefreshTotal();
            }
        }



        private void RefreshTable()
        {
            _TableItens.Clear();
            _PaginationItens.Clear();
            _ButtonSave.IsEnabled = (Itens.Count > 0) && (EditarOrcamento ?? NovoOrcamento).Status != CompraType.CompraStatus.Recebida;

            if (Itens.Count > 0)
            {
                Row currentRow;
                int num = 1;

                _StackEmpty.Visibility = Visibility.Collapsed;

                foreach (ItemCompraType item in Itens)
                {
                    currentRow = new Row()
                                .TD(Str.ZeroFill(num))
                                .TD(item.Produto.Name)
                                .TD(Str.Currency(item.Produto.Valor))
                                .TD(Str.ZeroFill(item.Quantidade))
                                .TD(Str.Currency(item.Produto.Valor * item.Quantidade));

                    _TableItens.Add(GetRowControl(currentRow, item));

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
            (EditarOrcamento ?? NovoOrcamento).ValorTotal = 0;

            foreach (ItemCompraType item in Itens)
                (EditarOrcamento ?? NovoOrcamento).ValorTotal += item.ValorTotal;

            _TextValorTotal.Text = Str.Currency((EditarOrcamento ?? NovoOrcamento).ValorTotal);

            System.Diagnostics.Trace.WriteLine((EditarOrcamento ?? NovoOrcamento).ValorTotal);
        }



        private Row GetRowControl(Row row, ItemCompraType item)
        {
            CompraType.CompraStatus status = (EditarOrcamento ?? NovoOrcamento).Status;

            if (status == CompraType.CompraStatus.Orcamento)
                return row.AC("X", Row.ActionLevel.Danger, sender => RemoveItemFromTable(item.Id));

            else if (status == CompraType.CompraStatus.Aprovada && item.Status == ItemCompraType.ICStatus.Aberto)
                return row.AC(Icon.Small("open-in-new"), Row.ActionLevel.Info, sender => ReceberItemFromtable(item));

            else if (status == CompraType.CompraStatus.Recebida || item.Status == ItemCompraType.ICStatus.Baixado)
                return row.AC(Icon.Small("package-check"), Row.ActionLevel.Info, sender => ExibeItensFromTable(item.Compra));

            return row.AC(Icon.Small("timer"), Row.ActionLevel.None);
        }



        private void RemoveItemFromTable(int id)
        {
            foreach (ItemCompraType ict in Itens)
            {
                if (ict.Id == id)
                {
                    Itens.Remove(ict);
                    break;
                }
            }

            RefreshTable();
            RefreshTotal();
            ShouldBeSavedEnabled();
        }



        private void ReceberItemFromtable(ItemCompraType item)
        {
            _GridDefault.Visibility = Visibility.Collapsed;
            _GridContainer.Children.Add(new BaixaCompraDialog(item, this));
        }



        private void ExibeItensFromTable(CompraType compra)
        {
            List<ItemType> itens = ItensViewModel.Read(new object[] { "compra", compra.Id });

            _GridDefault.Visibility = Visibility.Collapsed;
            _GridContainer.Children.Add(new ItensDialog(itens, this));
        }



        private bool OrcamentoHasSaved()
        {
            if (EditarOrcamento != null)
            {
                return ComprasViewModel.Update(EditarOrcamento) > 0;
            }
            else
                return ComprasViewModel.Create(NovoOrcamento);
        }



        private bool AllItensSaved()
        {
            int savedCount;

            ItemCompraType item;
            CompraType last = ComprasViewModel.Last() ?? new CompraType() { Id = 0 };
            List<ItemCompraType> CurrentItens = ItensComprasViewModel.Read(new object[] { "compra", last.Id });

            if (CurrentItens.Count > 0)
                foreach (ItemCompraType itc in CurrentItens)
                    ItensComprasViewModel.Delete(itc.Id);

            for (savedCount = 0; savedCount < Itens.Count; savedCount++)
            {
                item = Itens[savedCount];
                item.Compra = last;

                if (!ItensComprasViewModel.Create(item))
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, $"Erro ao salvar o item \"{item.Produto.Name}\""));
                    break;
                }
            }

            return savedCount == Itens.Count;
        }



        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (EditarOrcamento != null)
            {
                EditarOrcamento.Status = CompraType.CompraStatus.Cancelada;

                if (ComprasViewModel.Update(EditarOrcamento) > 0)
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Orçamento cancelado com sucesso!"));
                    RefreshControls();
                    RefreshTable();
                }
            }
        }



        private void ButtonAprovar_Click(object sender, RoutedEventArgs e)
        {
            if (EditarOrcamento != null)
            {
                EditarOrcamento.Status = CompraType.CompraStatus.Aprovada;

                if (ComprasViewModel.Update(EditarOrcamento) > 0)
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Orçamento aprovado com sucesso!"));
                    RefreshControls();
                    RefreshTable();
                }
            }
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
                        NovoOrcamento.Fornecedor = fornecedor.Value;
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
                    {
                        CurrentItem.Produto = produto;
                        _InputQuantidade.IsEnabled = true;
                    }
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
                CurrentItem.ValorTotal = CurrentItem.Produto.Valor * CurrentItem.Quantidade;
            }

            RefreshTotal();
            ShouldBeSavedEnabled();
        }



        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (NovoOrcamento.Fornecedor.Id <= 0)
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
                    Compra = (EditarOrcamento ?? new() { Id = 0 }),
                    Produto = new() { Id = 0 },
                    Quantidade = 0,
                    ValorTotal = 0
                };

                LoadMarcas();
                LoadProdutos(0);
                RefreshTable();
                RefreshTotal();

                _InputQuantidade.Zero(true);
            }

            ShouldBeSavedEnabled();
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (NovoOrcamento.Fornecedor.Id <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de selecionar o fornecedor"));

            else if (Itens == null || Itens.Count <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa adicionar ao menos um item"));

            else if (!OrcamentoHasSaved())
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao salvar o orçamento"));

            else if (AllItensSaved())
            {
                LoadFornecedores();
                LoadMarcas();
                LoadProdutos(0);

                Itens.Clear();

                RefreshTable();
                RefreshTotal();

                _InputQuantidade.Zero();

                NovoOrcamento = new()
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
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao salvar os itens da compra"));
        }



        public void BackToMain(object sender)
        {
            _GridContainer.Children.Remove((UIElement)sender);
            _GridDefault.Visibility = Visibility.Visible;

            RefreshItens();
            RefreshTable();
            RefreshTotal();
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
