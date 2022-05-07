using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Badges;
using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ItensDialog : Border
    {
        ProdutoType Produto { get; set; }
        Dictionary<string, ItemType[]> Itens { get; set; } 



        public ItensDialog()
        {
            Produto = new();
            Itens = new();

            InitializeComponent();
        }



        public ItensDialog(ProdutoType produto)
        {
            Produto = produto;
            Itens = new();

            InitializeComponent();

            Loaded += delegate
            {
                LoadItens();
                LoadBadges();
                LoadTable();
            };
        }



        private void LoadItens()
        {
            ItemType[] itens;

            foreach (string status in ItemType.STATUS)
            {
                itens = ItensViewModel.ItensPorProdutoByStatus(Produto.Id, status).ToArray();

                if (itens != null && itens.Length > 0)
                    Itens.Add(status, itens);
            }

            System.Diagnostics.Trace.WriteLine(Itens.Count);
        }



        private void LoadBadges()
        {
            string status;

            int  s;
            for (s = 0; s < ItemType.STATUS.Length; s++)
            {
                status = ItemType.STATUS[s];

                if (Itens.TryGetValue(status, out ItemType[]? tipos))
                {
                    _StackBadges.Children.Add(
                        new BadgeSecondary()
                        {
                            Content = $"{ status } ({ Str.ZeroFill(tipos.Length) })",
                            IsEnabled = (s == 0)
                        }
                    );
                }
            }
        }



        private void LoadTable()
        {
            /*
            _TableItens.Headers(
                        Header.Auto("Cod."),
                        Header.Auto("MAC"),
                        Header.Max("Almoxarifado"),
                        Header.Max("Descrição"),
                        Header.Auto("Status")
                    );

            foreach (ItemType item in Itens)
                _TableItens.Add(
                        new Row()
                            .TD(item.Codigo)
                            .TD(item.Mac)
                            .TD(item.Almoxarifado.Name)
                            .TD(item.Description)
                            .TD(item.Localizado)
                    );

            _Pagination.Table = _TableItens;
            _Pagination.Paginate();
            */
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
