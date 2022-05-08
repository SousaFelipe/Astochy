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
        int CurrentBadgeIndex;



        public ItensDialog()
        {
            Produto = new();
            Itens = new();
            Badges = new();
            CurrentBadgeIndex = -1;

            InitializeComponent();
        }



        public ItensDialog(ProdutoType produto)
        {
            Produto = produto;
            Itens = new();
            Badges = new();
            CurrentBadgeIndex = -1;

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
            Button button;
            string status;

            for (int s = 0; s < ItemType.STATUS.Length; s++)
            {
                status = ItemType.STATUS[s];

                if (Itens.TryGetValue(status, out ItemType[]? tipos))
                {
                    button = new()
                    {
                        Content = $"{ status } ({ Str.ZeroFill(tipos.Length) })",
                        Tag = status
                    };

                    button.Click += delegate { SelectBadge(s); };

                    _StackBadges.Children.Add(button);
                    Badges.Add(button);
                }
            }

            SelectBadge(0);
        }



        private void SelectBadge(int badgeIndex)
        {
            if (badgeIndex != CurrentBadgeIndex)
            {
                foreach (Button btn in Badges)
                    btn.Style = (Style)FindResource("BadgeGray");

                Button button = Badges[badgeIndex];
                button.Style = (Style)FindResource("BadgeSecondary");

                LoadTable(button.Tag.ToString());

                CurrentBadgeIndex = badgeIndex;
            }
        }



        private void LoadTable(string status)
        {
            _TableItens.Clear();

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

                System.Diagnostics.Trace.WriteLine(item.Mac);
            }

            _Pagination.Paginate();
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
