using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;
using VadenStock.View.Components.Containers;
using VadenStock.View.Models;
using VadenStock.View.Components.Widgets;
using VadenStock.View.Dialogs;

namespace VadenStock.View
{
    public partial class Fornecedores : UserControl
    {
        private List<FornecedorType> Dataset = new();



        public Fornecedores()
        {
            InitializeComponent();

            Loaded += delegate
            {
                Dataset = FornecedoresViewModel.TodosOsFornecedores;

                InitTable();
                RefreshTable();
            };
        }



        private void InitTable()
        {
            _TableFornecedores.Headers(
                        Header.Auto("ID"),
                        Header.Auto("CNPJ"),
                        Header.Auto("FANTASIA"),
                        Header.Max("EMAIL"),
                        Header.Max("TELEFONE"),
                        Header.Max("WHATSAPP"),
                        Header.Auto("AÇÃO")
                    );

            _PaginationFornecedores.Table = _TableFornecedores;
        }



        private void RefreshTable(List<FornecedorType>? fornecedores = null)
        {
            List<FornecedorType> fill = fornecedores ?? Dataset;

            if (fill != null && fill.Count > 0)
            {
                _TableFornecedores.Clear();
                _PaginationFornecedores.Clear();
                _StackEmpty.Visibility = Visibility.Collapsed;

                foreach (FornecedorType fornecedor in fill)
                {
                    _TableFornecedores.Add(
                        new Row()
                            .TD(fornecedor.Id)
                            .TD(fornecedor.Cnpj)
                            .TD(fornecedor.Fantasia)
                            .TD(fornecedor.Email)
                            .TD(fornecedor.Telefone)
                            .TD(fornecedor.Whatsapp)
                            .AC(Icon.Small("delete", "white"), Row.ActionLevel.Danger, sender =>
                            {
                                MainWindow window = (MainWindow)Application.Current.MainWindow;
                                window.DisplayDialog(new ConfirmDialog(ConfirmDialog.ConfirmType.Delete, "Você realmente deseja remover este registro?").OnConfirm(sender => DeleteFornecedor(fornecedor.Id)));
                            })
                    );
                }

                _TableFornecedores.Draw();
                _PaginationFornecedores.Paginate();
            }
            else
            {
                _StackEmpty.Visibility = Visibility.Visible;
            }
        }



        private void DeleteFornecedor(int id)
        {
            if (FornecedoresViewModel.Delete(id))
            {
                Dataset = FornecedoresViewModel.TodosOsFornecedores;
                RefreshTable();
            }
            else
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao remover registro"));
            }
        }



        private void InputBusca_Changed(object sender, TextChangedEventArgs e)
        {

        }



        private void ButtonNovoFornecedor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            window.DisplayDialog(new FornecedorDialog(), sender => {
                Dataset = FornecedoresViewModel.TodosOsFornecedores;
                RefreshTable();
            });
        }
    }
}
