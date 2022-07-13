using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Adapters;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class AlmoxarifadoDialog : Border
    {
        private AlmoxType Almox;
        private AlmoxType AlmoxNew;

        private List<ProdutoType> Produtos;
        private Dictionary<string, ItemType[]> Itens;

        private readonly List<Button> Badges;



        public AlmoxarifadoDialog(AlmoxType almoxarifado)
        {
            Almox = almoxarifado;
            AlmoxNew = almoxarifado;

            Produtos = new();
            Itens = new();

            Badges = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadDetails();
                LoadTipos();
                LoadProdutos();
                LoadBadges();
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
                        Header.Max("Produto"),
                        Header.Auto("Ação")
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
            }
        }



        private void LoadBadges()
        {
            string produto;
            ItemType[] itens;
            Button buttonBadge;

            Badges.Clear();
            _StackBadges.Children.Clear();

            for (int p = 0; p < Produtos.Count; p++)
            {
                produto = Produtos[p].Name;

                if (Itens.ContainsKey(produto))
                {
                    itens = Itens[produto];

                    if (itens != null && itens.Length > 0)
                    {
                        buttonBadge = new()
                        {
                            Margin = new Thickness(2, 0, 2, 0),
                            Content = $"{produto} ({Str.ZeroFill(itens.Length)})",
                            Tag = produto
                        };

                        buttonBadge.Click += SelectBadge;

                        _StackBadges.Children.Add(buttonBadge);
                        Badges.Add(buttonBadge);
                    }
                }
            }

            SelectBadge(null, null);
        }




        private void SelectBadge(object? sender, RoutedEventArgs? e)
        {
            foreach (Button btn in Badges)
                btn.Style = (Style)FindResource("BadgeGray");

            Button button = (Button)(sender ?? Badges[0]);
            button.Style = (Style)FindResource("BadgeSecondary");

            RefreshTable(button.Tag.ToString());
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
                                    .TD(Str.MAC(item.Mac))
                                    .TD(item.Produto.Name)
                                    .AC(new Image()
                                    {
                                        Width = 18,
                                        Height = 18,
                                        HorizontalAlignment = HorizontalAlignment.Center,
                                        VerticalAlignment = VerticalAlignment.Center,
                                        Source = Src.Icon("black-history")
                                    },
                                    Row.ActionLevel.Info,
                                    delegate
                                    {
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



        private void InputAlmoxName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            AlmoxNew.Name = input.Text.Trim();
            ShouldBeEnabledSave();
        }



        private void SelectAcao_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {

            }
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
