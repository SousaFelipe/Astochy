using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.View.Models;
using VadenStock.View.Structs;
using VadenStock.View.Components.Cards;

using VadenStock.View.Dialogs;
using VadenStock.View.Components.Forms;

using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();

            Loaded += delegate {

                if (ConfigsViewModel.Configured())
                    LoadAll();
                
                else
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.DisplayDialog(new ConfigDialog(), delegate
                    {
                        LoadAll();
                        return true;
                    });
                }
            };
        }



        private void LoadAll()
        {
            LoadCardPatrimonio();
            LoadAlmoxCards();
            RefreshChartByMarcas();
        }



        public void LoadCardPatrimonio()
        {
            PatrimonioStruct patrimonio = DashboardViewModel.GetPatrimonios();

            _PatrimonioEmEstoque.UpdateValue(patrimonio.Estoque, patrimonio.Total);
            _PatrimonioEmComodato.UpdateValue(patrimonio.Comodato, patrimonio.Total);
            _PatrimonioEmRota.UpdateValue(patrimonio.EmRota, patrimonio.Total);
        }



        private void LoadAlmoxCards()
        {
            List<AlmoxType> almoxarifados = AlmoxarifadosViewModel.TodosOsAlmoxarifados;

            _GridAlmoxarifados.RowDefinitions.Add(new RowDefinition());

            if (almoxarifados != null)
            {
                AlmoxType currentAlmo;
                ThumbnailCard currentCard;

                int r = 0;
                int l = 0;

                for (int a = 0; a < almoxarifados.Count; a++)
                {
                    currentAlmo = almoxarifados[a];
                    currentCard = Molecules.AlmoxarifadoThumbCard(currentAlmo, a, r);

                    _GridAlmoxarifados.Children.Add(currentCard);
                     Grid.SetColumn(currentCard, r);
                     Grid.SetRow(currentCard, l);

                    if (0 == (r - 3))
                    {
                        _GridAlmoxarifados.RowDefinitions.Add(new RowDefinition());

                        r = 0;
                        l++;
                    }
                    else r++;
                }
            }
        }



        private void RefreshChartByMarcas()
        {
            MarcaType current;
            List<ProdutoType> produtos;
            List<MarcaType> marcas = MarcasViewModel.TodasAsMarcas;

            string[] labels = new string[marcas.Count];
            double[] values = new double[marcas.Count];

            for (int i = 0; i < marcas.Count; i++)
            {
                current = marcas[i];
                labels[i] = current.Name[..3];
                produtos = ProdutosViewModel.ProdutosPorMarca(current.Id);

                for (int p = 0; p < produtos.Count; p++)
                    values[i] += ItensViewModel.CountItensPorProduto(produtos[p].Id);
            }

            _ChartEstoqueNivel.Clear();
            _ChartEstoqueNivel.SetSeries(values);
            _ChartEstoqueNivel.SetLabels(labels);
            _ChartEstoqueNivel.Draw();

            VadenStock.Http.Cliente cliente = new();
            VadenStock.Core.Http.Response res = cliente.Where("id", 1251).Get(10).Result;
        }



        private void RefreshChartByCategorias()
        {
            CategoriaType current;
            List<ProdutoType> produtos;
            List<CategoriaType> categorias = CategoriasViewModel.TodasAsCategorias;

            string[] labels = new string[categorias.Count];
            double[] values = new double[categorias.Count];

            for (int i = 0; i < categorias.Count; i++)
            {
                current = categorias[i];
                labels[i] = current.Name[..3];
                produtos = ProdutosViewModel.ProdutosPorCategoria(current.Id);

                for (int p = 0; p < produtos.Count; p++)
                    values[i] += ItensViewModel.CountItensPorProduto(produtos[p].Id);
            }

            _ChartEstoqueNivel.Clear();
            _ChartEstoqueNivel.SetSeries(values);
            _ChartEstoqueNivel.SetLabels(labels);
            _ChartEstoqueNivel.Draw();
        }



        private void CardNivelCheckChange(object sender, RoutedEventArgs e)
        {
            Radio radio = (Radio)sender;
            string tag = radio.Tag.ToString() ?? string.Empty;

            if (_ChartEstoqueNivel != null)
            {
                if (tag != null && tag.Equals("C"))
                    RefreshChartByCategorias();
                else
                    RefreshChartByMarcas();
            }
        }



        private void ButtonTransf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new TransferenciaDialog());
        }



        private void ButtonEntrada_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new EntradaDialog());
        }



        private void ButtonSaida_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new EntradaDialog());
        }



        private static class Molecules
        {
            public static ThumbnailCard AlmoxarifadoThumbCard(AlmoxType almox, int position, int rounds)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                int itens = ItensViewModel.CountItensPorAlmoxarifado(almox.Id);

                ThumbnailCard thumb = new()
                {
                    Margin = new Thickness(6, (position > 0 && ((rounds - 3) == -3)) ? 12 : 0, 6, 0),
                    Body = "#FFFFFF",
                    Header = almox.Name,
                    HeaderSize = 16,
                    SubHeader = (Str.ZeroFill(itens) + " item".Pluralize(itens, "n")),
                    SubHeaderSize = 12,
                };

                thumb.SetThumb(almox.GetIcon());

                thumb.SetHeaderAction((object sender) =>
                {
                    window.DisplayDialog(new AlmoxarifadoDialog(almox));
                    return true;
                });

                return thumb;
            }
        }
    }
}
