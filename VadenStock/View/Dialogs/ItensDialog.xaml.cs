using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ItensDialog : Border
    {
        ProdutoType Produto { get; set; }
        Dictionary<string, ItemType[]> Itens { get; set; }



        readonly List<Button> Badges;



        public ItensDialog()
        {
            Produto = new();
            Itens = new();
            Badges = new();

            InitializeComponent();
        }



        public ItensDialog(ProdutoType produto)
        {
            Produto = produto;
            Itens = new();
            Badges = new();

            InitializeComponent();

            Loaded += delegate
            {
                InitTable();
                LoadItens();
                LoadBadges();
            };
        }



        private void InitTable()
        {
            _TableItens.Headers(
                        Header.Auto("Cod."),
                        Header.Auto("MAC"),
                        Header.Max("Almoxarifado"),
                        Header.Max("Descrição"),
                        Header.Auto("Status")
                    );

            _Pagination.Table = _TableItens;
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
        }



        private void LoadBadges()
        {
            string status;
            ItemType[] itens;
            Button buttonBadge;

            for (int s = 0; s < ItemType.STATUS.Length; s++)
            {
                status = ItemType.STATUS[s];
                
                if (Itens.ContainsKey(status))
                {
                    itens = Itens[status];

                    if (itens != null && itens.Length > 0)
                    {
                        buttonBadge = new()
                        {
                            Margin = new Thickness(2, 0, 2, 0),
                            Content = $"{ status } ({ Str.ZeroFill(itens.Length) })",
                            Tag = status
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

            LoadTable(button.Tag.ToString());
            UpdateResume(button.Tag.ToString());
        }



        private void LoadTable(string status)
        {
            _TableItens.Clear();
            _Pagination.Clear();

            ItemType[] itens = Itens[status];

            foreach (ItemType item in itens)
            {
                _TableItens.Add(
                        new Row()
                            .TD(item.Codigo)
                            .TD(item.Mac)
                            .TD(item.Almoxarifado.Name)
                            .TD(item.Description)
                            .TD(item.Localizado)
                    );
            }

            _Pagination.Paginate();
        }



        private void UpdateResume(string status)
        {
            ItemType[] itens = Itens[status];
            int quantidade = 0;
            decimal valor = 00;

            foreach (ItemType item in itens)
            {
                quantidade++;
                valor += item.Produto.Price;
            }

            _TextQuantItens.Text = Str.ZeroFill(quantidade);
            _TextLabelQuantItens.Text = (" item".Pluralize(quantidade, "n") + ", totalizando ");
            _TextTotalItens.Text = ("R$ " + Str.Currency((valor * 100).ToString()));
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
