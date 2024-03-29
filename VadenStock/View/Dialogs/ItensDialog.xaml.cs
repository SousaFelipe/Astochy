﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.Tools;

using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Widgets;
using VadenStock.View.Models;
using System.Linq;
using System.Reflection;

namespace VadenStock.View.Dialogs
{
    public partial class ItensDialog : Border
    {
        public object? View { get; private set; }



        ProdutoType Produto { get; set; }
        List<string> Status { get; set; }
        Dictionary<string, ItemType[]> Itens { get; set; }



        public ItensDialog()
        {
            Produto = new();
            Status = new();
            Itens = new();

            InitializeComponent();
        }



        public ItensDialog(ProdutoType produto)
        {
            Produto = produto;
            Status = new();
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadItens();
                LoadStatus();
            };
        }



        public ItensDialog(List<ItemType> itens, object? parent = null)
        {
            Produto = new ProdutoType() { Id = 0 };
            Status = new();
            Itens = new();
            View = parent;

            InitializeComponent();

            Loaded += delegate
            {
                if (View != null)
                {
                    _ButtonBack.Visibility = Visibility.Visible;
                    _ButtonClose.Visibility = Visibility.Collapsed;

                    VerticalAlignment = VerticalAlignment.Top;
                }

                InitTable();
                LoadItens(itens);
                LoadStatus();
            };
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("COD."),
                        Header.Auto("MAC"),
                        Header.Max("ALMOXARIFADO"),
                        Header.Max("TRANSFERÊNCIA"),
                        Header.Auto("AÇÃO")
                    );

            _Pagination.Table = _TableItens;
        }



        private void LoadItens(List<ItemType>? itemList = null)
        {
            ItemType[] itens;

            foreach (string status in ItemType.STATUS)
            {
                if (itemList == null)
                    itens = ItensViewModel.ItensPorProdutoByStatus(Produto.Id, status).ToArray();
                
                else
                    itens = itemList.Where(i => i.Localizado.Equals(ItemType.GetStatus(status))).ToArray();

                if (itens != null && itens.Length > 0)
                {
                    Status.Add(status);
                    Itens.Add(status, itens);
                }
            }
        }



        private void LoadStatus()
        {
            foreach(string status in Status)
            {
                _SelectStatus.Items.Add(new ComboBoxItem()
                {
                    Tag = status,
                    Content = $"{status} ({Str.ZeroFill(Itens[status].Length)})"
                });
            }

            if (Status.Count > 0)
                (_SelectStatus.Find(Status[0]) ?? new ComboBoxItem()).IsSelected = true;
        }



        private void RefreshTable()
        {
            string status = (string)((ComboBoxItem)_SelectStatus.SelectedItem).Tag;

            _TableItens.Clear();
            _Pagination.Clear();

            if (!string.IsNullOrEmpty(status))
            {
                ItemType[] itens = Itens[status];

                _StackEmpty.Visibility = Visibility.Collapsed;

                foreach (ItemType item in itens)
                {
                    _TableItens.Add(
                            new Row()
                                .TD(item.Codigo)
                                .TD(Str.MAC(item.Mac))
                                .TD(item.Almoxarifado.Name)
                                .TD(item.UltimaTransf != null ? item.UltimaTransf.Value.ToString("dd/MM/yyyy") : string.Empty)
                                .AC(Icon.Small("history"), Row.ActionLevel.None, sender => {
                                    _GridDefault.Visibility = Visibility.Collapsed;
                                    _GridContainer.Children.Add(new HistoricoDialog(item, this));
                                })
                        );
                }

                _TableItens.Draw();
                _Pagination.Paginate();
            }
            else
                _StackEmpty.Visibility = Visibility.Visible;
        }



        private void UpdateResume()
        {
            string status = (string)((ComboBoxItem)_SelectStatus.SelectedItem).Tag;
            int quantidade = 0;
            double valor = 0;

            if (!string.IsNullOrEmpty(status))
            {
                ItemType[] itens = Itens[status];

                foreach (ItemType item in itens)
                {
                    quantidade++;
                    valor += item.Produto.Valor;
                }
            }

            _TextQuantItens.Text = Str.ZeroFill(quantidade);
            _TextLabelQuantItens.Text = (" item".Pluralize(quantidade, "n") + ", totalizando ");
            _TextTotalItens.Text = ("R$ " + Str.Currency(valor));
        }



        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MethodInfo? method = View?.GetType().GetMethod("BackToMain");
            method?.Invoke(View, new object?[] { this });
        }



        private void SelectStatus_Changed(object sender, SelectionChangedEventArgs e)
        {
            RefreshTable();
            UpdateResume();
        }



        public void BackToMain(object sender)
        {
            _GridContainer.Children.Remove((UIElement)sender);
            _GridDefault.Visibility = Visibility.Visible;
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
