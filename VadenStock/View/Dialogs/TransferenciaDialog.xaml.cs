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
    public partial class TransferenciaDialog : Border
    {
        List<ItemType?> Itens { get; set; }



        AlmoxType? Origem;
        AlmoxType? Destino;



        public TransferenciaDialog()
        {
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                LoadAlmoxOrigem();
                InitTable();
            };
        }



        private void LoadAlmoxOrigem()
        {
            List<AlmoxType> almoxarifados = AlmoxarifadosViewModel.TodosOsAlmoxarifados;

            _ComboOrigem.Clear(true);

            foreach (AlmoxType a in almoxarifados)
            {
                _ComboOrigem.Items.Add(new ComboBoxItem()
                {
                    Tag = a.Id,
                    Content = a.Name
                });
            }
        }



        private void LoadAlmoxDestino()
        {
            List<AlmoxType> almoxarifados = AlmoxarifadosViewModel.TodosOsAlmoxarifados;

            _ComboDestino.Clear(true);

            foreach (AlmoxType a in almoxarifados)
            {
                if (Origem != null && a.Id != Origem.Value.Id)
                {
                    _ComboDestino.Items.Add(new ComboBoxItem()
                    {
                        Tag = a.Id,
                        Content = a.Name
                    });
                }
            }
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("Cod."),
                        Header.Auto("MAC"),
                        Header.Max("Produto"),
                        Header.Auto("Status"),
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

                foreach (ItemType? item in Itens)
                {
                    if (item != null)
                    {
                        _TableItens.Add(
                            new Row()
                                .TD(item.Value.Codigo)
                                .TD(Str.MAC(item.Value.Mac))
                                .TD(item.Value.Produto.Name)
                                .TD(item.Value.Localizado)
                                .AC("X", Row.ActionLevel.Danger, sender => RemoveItemFromTransferencia(item.Value.Id))
                        );
                    }
                }

                _PaginationItens.Paginate();
            }
            else
            {
                _StackEmpty.Visibility = Visibility.Visible;
            }
        }



        private bool RemoveItemFromTransferencia(int id)
        {
            foreach (ItemType? item in Itens)
            {
                if (item != null && item.Value.Id == id)
                {
                    Itens.Remove(item);
                    RefreshTable();
                    return true;
                }
            }

            return false;
        }



        private void ComboOrigem_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    Origem = AlmoxarifadosViewModel.Find(new object[] { "id", tag });
                    _ComboDestino.IsEnabled = true;
                    LoadAlmoxDestino();
                }
                else
                {
                    Origem = null;
                    _ComboDestino.Clear(true);
                    _ComboDestino.IsEnabled = false;
                }
            }
        }



        private void ComboDestino_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                int tag = Convert.ToInt32(item.Tag);

                if (tag > 0)
                {
                    Destino = AlmoxarifadosViewModel.Find(new object[] { "id", tag });
                    _InputCodigo.IsEnabled = true;
                }
                else
                {
                    Destino = null;
                    _InputCodigo.IsEnabled = false;
                    _InputCodigo.Text = string.Empty;
                }
            }
        }



        private void InputCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string search = input.Text;

            if (!string.IsNullOrEmpty(search) && search.Length >= 4)
            {
                ItemType? item = ItensViewModel.Find(search);

                if (item != null)
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;

                    if (Origem != null && Destino != null && (item.Value.Almoxarifado.Id == Origem.Value.Id))
                    {
                        if (Itens.Contains(item))
                            window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Info, "O item já foi adicionado à lista de transferência"));
                        
                        else
                        {
                            Itens.Add(item);
                            RefreshTable();
                        }
                    }
                    else
                    {
                        window.DisplayAlert(
                            new AlertDialog(AlertDialog.AlertType.Info, "O item não se encontra no Almoxarifado de Origem")
                            );
                    }

                    input.Text = string.Empty;
                }
            }
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (Origem == null)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Selecione o almoxarifado de Origem"));

            else if (Destino == null)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Selecione o almoxarifado de Destino"));

            else if (Itens.Count <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Uma transferência não pode ser registrada sem itens"));

            else
            {
                if (AlmoxarifadosViewModel.Transferir(Origem, Destino, Itens))
                {
                    _ComboOrigem.Clear(true);
                    _ComboDestino.Clear(true);
                    _TableItens.Clear();
                    _PaginationItens.Clear();
                    _ButtonSave.IsEnabled = false;
                    Itens.Clear();
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Transferência realizada com sucesso"));
                }
                else
                    window.DisplayDialog(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao realizar transferência"));
            }
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
