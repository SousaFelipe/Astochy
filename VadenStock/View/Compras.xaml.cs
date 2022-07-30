using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;
using VadenStock.View.Adapters;
using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Widgets;
using VadenStock.View.Models;
using VadenStock.Tools;
using VadenStock.View.Dialogs;



namespace VadenStock.View
{
    public partial class Compras : UserControl
    {
        private CompraType Filtro;

        private DateTime? FiltroDataDe;
        private DateTime? FiltroDataAte;



        public Compras()
        {
            Filtro = new()
            {
                NumSerie = string.Empty,
                Status = CompraType.CompraStatus.Indefinido,
            };

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadOrcamentos();
                RefreshCompras();
                LoadFornecedores();
            };
        }



        private void InitTable()
        {
            _TableCompras.Headers(
                        Header.Auto("NS"),
                        Header.Max("FORNECEDOR"),
                        Header.Auto("VALOR"),
                        Header.Auto("ITENS"),
                        Header.Max("STATUS"),
                        Header.Max("EMISSÃO"),
                        Header.Auto("AÇÃO")
                    );

            _PaginationCompras.Table = _TableCompras;
        }



        private void LoadOrcamentos()
        {
            List<CompraType> compras = Compra.Model
                .Where("id", ">", 1)
                .Where("status", "=", "Orcamento")
                .Select();

            OrcamentosAdapter adapter = new(_GridOrcamentos);

            if (compras.Count > 0)
            {
                adapter.Update(compras);
                _StackOrcamentosEmpty.Visibility = Visibility.Collapsed;
            }
            else
            {
                adapter.Clear();
                _StackOrcamentosEmpty.Visibility = Visibility.Visible;
            }
        }



        private void RefreshCompras()
        {
            string[] months = new string[12];
            double[] values = new double[12];

            List<CompraType> compras;

            for (int m = 0; m < 12; m++)
            {
                months[m] = DateTime.Now.AddMonths(m - 11).ToString("MM/yyyy");

                compras = Compra.Model
                    .Where("status", "Finalizada")
                    .Where("updated_at", "LIKE", months[m])
                    .Select();

                if (compras != null)
                    foreach (CompraType compra in compras)
                        values[m] += compra.ValorTotal;

                else
                    values[m] = 0.00;
            }

            _ChartCompras.Clear();
            _ChartCompras.SetSeries(values);
            _ChartCompras.SetLabels(months);
            _ChartCompras.Draw();
        }



        private void LoadFornecedores()
        {
            List<FornecedorType> fornecedores = FornecedoresViewModel.TodosOsFornecedores;

            _SelectFornecedores.Clear(true);

            foreach (FornecedorType fornecedor in fornecedores)
            {
                _SelectFornecedores.Items.Add(new ComboBoxItem()
                {
                    Tag = fornecedor.Id,
                    Content = fornecedor.Tag
                });
            }
        }



        private void RefreshTable()
        {
            if (_TableCompras != null)
            {
                List<CompraType> compras = GetFilteredDataset();

                _TableCompras.Clear();
                _PaginationCompras.Clear();

                if (compras != null && compras.Count > 0)
                {
                    List<ItemType> itens;

                    foreach (CompraType compra in compras)
                    {
                        _StackEmpty.Visibility = Visibility.Collapsed;

                        itens = ItensViewModel.Read(new string[] { "compra", compra.Id.ToString() });

                        _TableCompras.Add(
                            new Row()
                                .TD(compra.NumSerie)
                                .TD(compra.Fornecedor.Tag)
                                .TD(Str.Currency(Convert.ToString(compra.ValorTotal)))
                                .TD(Str.ZeroFill(itens.Count))
                                .TD(CompraType.GetStatusName(compra.Status))
                                .TD((compra.DataEmissao ?? compra.CreatedDate).ToString("dd/MM/yyyy HH:mm").Replace(" ", " às "))
                                .AC(Icon.Small("history"), Row.ActionLevel.Info, sender => {

                                })
                        );

                        _PaginationCompras.Paginate();
                    }
                }
                else
                {
                    _StackEmpty.Visibility = Visibility.Visible;
                }
            }
        }



        private List<CompraType> GetFilteredDataset()
        {
            Compra model = Compra.Model.Where("id", ">", 1);

            if (!string.IsNullOrEmpty(Filtro.NumSerie))
                model.Where("ns", "LIKE", Filtro.NumSerie);

            if (Filtro.Status != CompraType.CompraStatus.Indefinido)
                model.Where("status", CompraType.GetStatusName(Filtro.Status));

            if (Filtro.Fornecedor.Id > 0)
                model.Where("fornecedor", Filtro.Fornecedor.Id);

            if (FiltroDataDe != null)
                model.Where("updated_at", ">=", FiltroDataDe.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            if (FiltroDataAte != null)
                model.Where("updated_at", ">=", FiltroDataAte.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            return model.Select();
        }



        private void ButtonNovoOrcamento_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new OrcamentoDialog(), sender => LoadOrcamentos());
        }



        private void InputSerie_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Filtro.NumSerie = input.Text;

            RefreshTable();
        }



        private void SelectStatus_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                string? tag = item.Tag.ToString();
                Filtro.Status = CompraType.GetStatus(tag ?? "Indefinido");
            }

            RefreshTable();
        }



        private void SelectFornecedores_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                FornecedorType? fornecedor = FornecedoresViewModel.Find(item.Tag);

                if (fornecedor != null)
                    Filtro.Fornecedor = fornecedor ?? new FornecedorType() { Id = 0 };
            }

            RefreshTable();
        }



        private void InputDateDe_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)sender;
            FiltroDataDe = picker.SelectedDate ?? null;

            RefreshTable();
        }



        private void InputDateAte_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)sender;
            FiltroDataAte = picker.SelectedDate ?? null;

            RefreshTable();
        }
    }
}
