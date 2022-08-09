using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Widgets;
using VadenStock.View.Models;
using VadenStock.View.Structs;



namespace VadenStock.View.Dialogs
{
    public partial class BaixaCompraDialog : Border
    {
        public object? View { get; private set; }



        private ItemCompraType Item;
        private List<ItemType> Itens;
        private ItemType SalvarItem;



        public BaixaCompraDialog(ItemCompraType item, object? parent = null)
        {
            Item = item;
            View = parent;
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {   
                if (View != null)
                {
                    _ButtonBack.Visibility = Visibility.Visible;
                    VerticalAlignment = VerticalAlignment.Top;
                }

                LoadProduto(item);
                InitItem();
                InitTable();
                RefreshTable();
                RefreshStatus();
            };
        }



        private void LoadProduto(ItemCompraType item)
        {
            ProdutoStruct produto = new(item.Produto);

            _BorderImage.Background = new ImageBrush()
            {
                ImageSource = Src.Storage($"{produto.Image.FileName}{produto.Image.FileExtension}")
            };

            _TextProduto.Text = Item.Produto.Name;
        }



        private void InitItem()
        {
            ConfigType? config = ConfigsViewModel.Default;
            AlmoxType? almoxarifado = config?.AlmoxPrincipal;

            SalvarItem = new()
            {
                Codigo = string.Empty,
                Mac = string.Empty,
                Produto = Item.Produto,
                Almoxarifado = almoxarifado ?? new AlmoxType() { Id = 1 },
                Compra = Item.Compra,
                Localizado = ItemType.Status.Estoque,
                UltimaTransf = DateTime.Now,
                Valor = Item.Produto.Valor,
            };

            Itens = ItensViewModel.Read(
                new object[] { "compra", SalvarItem.Compra.Id },
                new object[] { "produto", Item.Produto.Id }
            );
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
                                .AC(Icon.Small("delete", "white"), Row.ActionLevel.Danger, sender => { })
                        );
                }

                _PaginationItens.Paginate();
            }
            else
            {
                _StackEmpty.Visibility = Visibility.Visible;
            }
        }



        private void RefreshStatus()
        {
            if (Itens.Count == 0)
            {
                _TextStatus.Text = "Nenhum item salvo...";
                return;
            }

            int save = Itens.Count;
            int rest = Item.Quantidade - save;

            if (save < Item.Quantidade)
            {
                _TextStatus.Text = $"{Str.ZeroFill(save)} {"item".Pluralize(save, "n")} {"registrado".Pluralize(save)}... Restam {Str.ZeroFill(rest)}";
            }
            else
            {
                _TextStatus.Text = "Todos os itens foram registrados";
                Item.Status = ItemCompraType.ICStatus.Baixado;

                if (ItensComprasViewModel.Update(Item) > 0)
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Todos os itens foram registrados com sucesso"));

                    _InputCodigo.IsEnabled = false;
                    _InputMAC.IsEnabled = false;
                    _ButtonEntrada.IsEnabled = false;
                }
            }
        }



        private bool SaveCurrentItem()
        {
            SalvarItem.UltimaTransf = DateTime.Now;

            if (ItensViewModel.Create(SalvarItem))
            {
                Itens.Add(SalvarItem);

                InitItem();
                RefreshStatus();
                RefreshTable();

                _InputCodigo.Clear();
                _InputMAC.Clear();
                _InputCodigo.Focus();

                return true;
            }

            return false;
        }



        private void InputCodigo_Change(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            SalvarItem.Codigo = input.Text.Trim();
        }



        private void InputMAC_Change(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            SalvarItem.Mac = input.Text.Replace(":", "").Trim();
        }



        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(SalvarItem.Codigo))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "O item não pode ser registrado sem um código"));

            else
            {
                ItemType? checkCode = ItensViewModel.Find(SalvarItem.Codigo);

                if (checkCode == null)
                {
                    ItemType? checkMAC = ItensViewModel.Find(
                        string.IsNullOrEmpty(SalvarItem.Mac)
                            ? "00:00:00:00:00:00"
                            : SalvarItem.Mac
                    );

                    if (checkMAC == null)
                    {
                        if (!SaveCurrentItem())
                            window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao registrar item"));
                    }
                    else
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Info, $"Já existe um item registrado com o MAC {Str.MAC(SalvarItem.Mac)}"));
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Info, $"Já existe um item registrado com o código {SalvarItem.Codigo}"));
            }
        }



        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MethodInfo? method = View?.GetType().GetMethod("BackToMain");
            method?.Invoke(View, new object?[] { this });
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
