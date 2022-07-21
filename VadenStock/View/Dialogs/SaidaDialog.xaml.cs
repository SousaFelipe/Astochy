using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Core.Http;

using VadenStock.Http;
using VadenStock.Model.Types;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Forms;
using VadenStock.View.Models;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class SaidaDialog : Border
    {
        List<ItemType?> Itens { get; set; }



        AlmoxType? Origem;
        AlmoxType? Destino;



        public SaidaDialog()
        {
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                _InputCliente.SetPopup(_BorderClientes);
                _InputCliente.SetContainer(_StackClientes);
                _InputCliente.OnSearch((result) => ProccessResult(result));

                InitTable();
            };
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



        private void ProccessResult(string result)
        {
            if (result.Length > 2)
            {
                Response response = Cliente.Conn.Where("razao", "LE", result).Get(10).Result;
                List<Cliente>? clientes = response.Registros.ToObject<List<Cliente>>();

                _StackClientes.Children.Clear();

                if (clientes != null)
                {
                    _BorderClientes.Visibility = Visibility.Visible;

                    foreach (Cliente c in clientes)
                    {
                        _StackClientes.Children.Add(
                                new ListItem($"auto:{c.id}", $"max:{c.razao}")
                                    .Action((object[] res) => _InputCliente.Select(res[1].ToString()))
                            );
                    }
                }
                else
                {
                    _BorderClientes.Visibility = Visibility.Collapsed;
                }
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

            if (Itens.Count == 0)
                Origem = null;

            return false;
        }



        private void SelectAcoes_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                string tag = item.Tag.ToString() ?? "Desconhecido";
                Destino = AlmoxarifadosViewModel.Find(
                    new object[] { "name", tag },
                    new object[] { "listagem", 0 }
                );
            }
        }



        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            string codigo = _InputCodigo.Text;

            if (!string.IsNullOrEmpty(codigo) && codigo.Length >= 4)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                ItemType? item = ItensViewModel.Find(codigo);

                if (item != null)
                {
                    if (Itens.Contains(item))
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Info, "O item já foi adicionado à lista"));

                    else if (Origem != null && Origem.Value.Id != item.Value.Almoxarifado.Id)
                        window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Os itens de uma saída devem pertencer ao mesmo Almoxarifado"));

                    else
                    {
                        Itens.Add(item);
                        RefreshTable();

                        if (Itens.Count == 1)
                        {
                            #pragma warning disable CS8629
                            int id = Itens[0].Value.Almoxarifado.Id;
                            Origem = AlmoxarifadosViewModel.Find(new object[] { "id", id });
                            #pragma warning restore CS8629
                        }
                    }

                    _InputCodigo.Text = string.Empty;
                    _InputCodigo.Focus();
                }
                else
                {
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, $"Nenhum ítem encontrado com o código { codigo }"));
                }
            }
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (Destino == null)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao carregar Almoxarifado de Comodato"));

            else if (AlmoxarifadosViewModel.Transferir(Origem, Destino, Itens))
            {
                SaidasViewModel.Create(new SaidaType()
                {
                    Transferencia = TransferenciasViewModel.Last,
                    Responsavel = _InputCliente.Text,
                    Tipo = Destino.Value.Acao
                });

                _InputCliente.Clear();
                _SelectAcoes.Clear(true);
                _InputCodigo.Clear();
                _TableItens.Clear();
                _PaginationItens.Clear();
                _ButtonSave.IsEnabled = false;
                Itens.Clear();

                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Saída registrada com sucesso"));
            }
            else
                window.DisplayDialog(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao registrar saída"));
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
