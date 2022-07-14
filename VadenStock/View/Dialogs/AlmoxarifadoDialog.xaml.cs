using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Widgets;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class AlmoxarifadoDialog : Border
    {
        private AlmoxType Almox;
        private AlmoxType AlmoxNew;

        private readonly List<ProdutoType> Produtos;
        private readonly Dictionary<string, ItemType[]> Itens;



        public AlmoxarifadoDialog(AlmoxType almoxarifado)
        {
            Almox = almoxarifado;
            AlmoxNew = almoxarifado;

            Produtos = new();
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadDetails();
                LoadTipos();
                LoadProdutos();
            };
        }



        private void InitTable()
        {
            _TableItens.SetOptions(new Options.TableOptions()
            {
                Stripped = true,
                DisplayRows = 5
            });

            _TableItens.Headers(
                        Header.Auto("Cod."),
                        Header.Auto("MAC"),
                        Header.Max("Entrada"),
                        Header.Auto("     ")
                    );

            _PaginationItens.Table = _TableItens;
        }



        private void LoadDetails()
        {
            _ImageAlmoxIcon.Source = Src.Image(
                    Almox.Tipo == AlmoxType.Hosted.Carro ? "64-car" : Almox.Tipo == AlmoxType.Hosted.Moto ? "64-moto" : "64-warehouse"
                );

            _TextAlmoxName.Text = Almox.Name;
        }



        private void LoadTipos()
        {
            _SelectTipo.Clear(true);

            Dictionary<string, string> tipos = new()
            {
                { "E", "Estoque" },
                { "C", "Carro" },
                { "M", "Moto" }
            };

            foreach (var tipo in tipos)
            {
                _SelectTipo.Items.Add(new ComboBoxItem()
                {
                    Tag = tipo.Key,
                    Content = tipo.Value
                });
            }

            foreach (ComboBoxItem item in _SelectTipo.Items)
                item.IsSelected = (AlmoxType.GetTipo(Almox.Tipo) == (string)item.Tag);
        }



        public void LoadProdutos()
        {
            List<ItemType> itens = ItensViewModel.ItensPorAlmoxarifado(Almox.Id);

            foreach (ItemType item in itens)
                if (!Produtos.Contains(item.Produto))
                    Produtos.Add(item.Produto);

            foreach (ProdutoType p in Produtos)
            {
                itens = ItensViewModel.ItensPorAlmoxarifado(Almox.Id, new object[] { "produto", p.Id });
                Itens.Add(p.Name, itens.ToArray());

                _SelectProdutos.Items.Add(new ComboBoxItem()
                {
                    Tag = p.Name,
                    Content = p.Name
                });
            }

            if (Produtos.Count > 0)
                _SelectProdutos.SelectedIndex = 1;
        }



        private void RefreshTable(string produto)
        {
            if (Itens.Count > 0)
            {
                ItemType[] itens = Itens[produto];

                if (itens.Length > 0)
                {
                    _TableItens.Clear();
                    _PaginationItens.Clear();
                    _StackEmpty.Visibility = Visibility.Collapsed;

                    foreach (ItemType item in itens)
                    {
                        _TableItens.Add(
                                new Row()
                                    .TD(item.Codigo)
                                    .TD(Str.MAC(item.Mac.Replace(":", "")))
                                    .TD(item.CreatedDate.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às "))
                                    .AC(Icon.Small("history"), Row.ActionLevel.Info, delegate
                                    {
                                        _GridDefault.Visibility = Visibility.Collapsed;
                                        _GridContainer.Children.Add(new HistoricoDialog(Almox, item, this));
                                        return true;
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
        }



        private void ShouldBeEnabledSave()
        {
            if (_ButtonSave != null)
            {
                _ButtonSave.IsEnabled = (
                    Almox.Tipo != AlmoxNew.Tipo ||
                    Almox.Acao != AlmoxNew.Acao ||
                    Almox.Name != AlmoxNew.Name ||
                    Almox.Description != AlmoxNew.Description
                );
            }
        }



        public void BackToMain(object sender)
        {
            _GridContainer.Children.Remove((UIElement)sender);
            _GridDefault.Visibility = Visibility.Visible;
        }



        private void InputAlmoxName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            AlmoxNew.Name = input.Text.Trim();
            ShouldBeEnabledSave();
        }



        private void SelectTipo_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                AlmoxNew.Tipo = AlmoxType.GetTipo((string)item.Tag);
                ShouldBeEnabledSave();
            }
        }



        private void SelectAcao_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {

            }
        }



        private void SelectProduto_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                string? produto = item.Tag.ToString();
                RefreshTable(produto);
            }
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
