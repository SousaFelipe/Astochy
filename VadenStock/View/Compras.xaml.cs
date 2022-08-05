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
        private readonly CompraType Filtro;

        private string? FiltroDataDe;
        private string? FiltroDataAte;



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
            _TableCompras.SetColors(
                "#FFFFFF",
                "#ECEFF1",
                "#DFE4E7",
                "#CFD8DC"
            );

            _TableCompras.Headers(
                        Header.Auto("NS"),
                        Header.Max("FORNECEDOR"),
                        Header.Max("VALOR"),
                        Header.Max("ITENS"),
                        Header.Max("STATUS"),
                        Header.Auto("EMISSÃO"),
                        Header.Auto("AÇÃO")
                    );

            _PaginationCompras.Table = _TableCompras;
        }



        public void LoadOrcamentos()
        {
            List<CompraType> compras = Compra.Model
                .Where("status", "Orcamento")
                .Or("status", "Aprovada")
                .Select();

            OrcamentosAdapter adapter = new(_GridOrcamentos);
            adapter.SetView(this);

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



        public void RefreshCompras()
        {
            Dictionary<string, string[]> dates = new();
            DateTime[] months = new DateTime[12];

            string[] labels = new string[12];
            double[] values = new double[12];
            string[] tags = new string[12];

            List<CompraType> compras;

            for (int m = 0; m < 12; m++)
            {
                months[m] = DateTime.Now.AddMonths(m - 11);
                labels[m] = months[m].ToString("MM/yyyy");

                dates.Add(
                    labels[m],
                    new string[]
                    {
                        Tmp.FirstDayOfMonth(months[m]).ToString("yyyy-MM-dd") + " 00:00:00",
                        Tmp.LastDayOfMonth(months[m]).ToString("yyyy-MM-dd") + " 23:59:00"
                    }
                );
            }

            int d = 0;
            foreach (var date in dates)
            {
                compras = Compra.Model
                    .Where("status", "!=", "Orcamento")
                    .Where("status", "!=", "Cancelada")
                    .Where("status", "!=", "Indefinido")
                    .Where("updated_at", ">=", date.Value[0])
                    .Where("updated_at", "<=", date.Value[1])
                    .Select();

                if (compras != null)
                {
                    foreach (CompraType compra in compras)
                        values[d] += compra.ValorTotal;

                    tags[d] = Str.Currency(values[d]);
                }

                else
                    values[d] = 0.00;

                d++;
            }

            _ChartCompras.Clear();
            _ChartCompras.SetSeries(values);
            _ChartCompras.SetLabels(labels);
            _ChartCompras.SetTags(tags);
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



        public void RefreshTable()
        {
            if (_TableCompras != null)
            {
                List<CompraType> compras = GetFilteredDataset();

                _TableCompras.Clear();
                _PaginationCompras.Clear();

                if (compras != null && compras.Count > 0)
                {
                    int itens = 0;

                    _StackEmpty.Visibility = Visibility.Collapsed;

                    foreach (CompraType compra in compras)
                    {
                        if (compra.Status == CompraType.CompraStatus.Recebida)
                            itens = ItensViewModel.Read(new object[] { "compra", compra.Id }).Count;

                        else
                        {
                            List<ItemCompraType> ictps = ItensComprasViewModel.Read(new object[] { "compra", compra.Id });

                            foreach (ItemCompraType itc in ictps)
                                itens += itc.Quantidade;
                        }

                        _TableCompras.Add(
                            new Row()
                                .TD(string.IsNullOrEmpty(compra.NumSerie) ? "-----" : compra.NumSerie)
                                .TD(compra.Fornecedor.Tag)
                                .TD(Str.Currency(compra.ValorTotal))
                                .TD(Str.ZeroFill(itens))
                                .TD(CompraType.GetStatusName(compra.Status))
                                .TD(GetDateFromStatusTable(compra))
                                .AC(Icon.Small("open-in-new"), Row.ActionLevel.None, sender =>
                                {
                                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                                    window.DisplayDialog(new OrcamentoDialog(compra));
                                })
                        );
                    }

                    _PaginationCompras.Paginate();
                }
                else
                {
                    _StackEmpty.Visibility = Visibility.Visible;
                }
            }
        }



        private List<CompraType> GetFilteredDataset()
        {
            Compra model = Compra.Model
                .Where("id", ">", 1)
                .Where("status", "!=", "Orcamento");

            if (!string.IsNullOrEmpty(Filtro.NumSerie))
                model.Where("ns", "LIKE", Filtro.NumSerie);

            if (Filtro.Status != CompraType.CompraStatus.Indefinido)
                model.Where("status", CompraType.GetStatusName(Filtro.Status));

            if (Filtro.Fornecedor.Id > 1)
                model.Where("fornecedor", Filtro.Fornecedor.Id);

            if (FiltroDataDe != null)
                model.Where("updated_at", ">=", FiltroDataDe);

            if (FiltroDataAte != null)
                model.Where("updated_at", "<=", FiltroDataAte);

            return model.OrderBy("id", "DESC");
        }



        private static string GetDateFromStatusTable(CompraType compra)
        {
            return (compra.Status == CompraType.CompraStatus.Recebida)
                ? (compra.DataEmissao ?? compra.CreatedDate).ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ")
                : "--/--/----";
        }



        private void ButtonNovoOrcamento_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new OrcamentoDialog(), sender =>
            {
                LoadOrcamentos();
                RefreshCompras();
                RefreshTable();
            });
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

                if (fornecedor != null && fornecedor.Value.Id > 1)
                    Filtro.Fornecedor = fornecedor.Value;

                else
                    Filtro.Fornecedor = new FornecedorType() { Id = 0 };
            }

            RefreshTable();
        }



        private void InputDateDe_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)sender;

            if (string.IsNullOrEmpty(picker.Text) || picker.Text.Length < 10)
                FiltroDataDe = null;

            else
            {
                string[] dateArray = picker.Text.Split('/');
                Array.Reverse(dateArray);
                FiltroDataDe = $"{string.Join('-', dateArray)} 00:00:00";
            }

            RefreshTable();
        }

        

        private void InputDateAte_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)sender;

            if (string.IsNullOrEmpty(picker.Text) || picker.Text.Length < 10)
                FiltroDataAte = null;

            else
            {
                string[] dateArray = picker.Text.Split('/');
                Array.Reverse(dateArray);
                FiltroDataAte = $"{string.Join('-', dateArray)} 23:59:00";
            }

            RefreshTable();
        }
    }
}
