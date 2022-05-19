using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Cards;
using VadenStock.View.Dialogs;

using VadenStock.Tools;



namespace VadenStock.View.Adapters
{
    public class ProdutosAdapter : GridAdapter
    {
        public List<ProdutoType> Dataset = new();
        public Dictionary<string, List<ProdutoType>> OrderedDataset = new();



        private Produtos? View;



        public ProdutosAdapter(Grid container) : base(container) { }



        public void SetView(Produtos view)
        {
            View = view;
        }



        public void Update(List<ProdutoType> dataset, bool buildAfterUpdate = true)
        {
            if (Dataset == null)
                Dataset = dataset;
            else
                Dataset.AddRange(dataset);

            OrderedDataset = ProdutosViewModel.OrderByMarca(Dataset);

            if (buildAfterUpdate)
                Build();
        }



        public override void Build(Func<UIElement, bool>? callback = null)
        {
            Clear();

            if (Container != null)
            {

                Container.RowDefinitions.Add(new RowDefinition());

                List<ProdutoType> currentDataset;
                ProdutoType currentProduto;
                TextBlock currentLabel;
                MidiaThumbCard currentCard;

                int rounds = 0;
                int crrRow = 0;

                foreach (string key in OrderedDataset.Keys)
                {
                    if (OrderedDataset[key] != null && OrderedDataset[key].Count > 0)
                    {
                        currentDataset = OrderedDataset[key];
                        currentLabel = CerateLabel(key);

                        Container.Children.Add(currentLabel);

                        System.Diagnostics.Trace.WriteLine(Container.RowDefinitions.Count);

                        Grid.SetColumn(currentLabel, 0);
                        Grid.SetColumnSpan(currentLabel, 4);
                        Grid.SetRow(currentLabel, crrRow);

                        crrRow++;

                        for (int i = 0; i < currentDataset.Count; i++)
                        {
                            currentProduto = currentDataset[i];
                            currentCard = CreateThumbCard(currentProduto);

                            Container.Children.Add(currentCard);

                            Grid.SetColumn(currentCard, rounds);
                            Grid.SetRow(currentCard, crrRow);

                            if (0 == (rounds - 3))
                            {
                                Container.RowDefinitions.Add(new RowDefinition());

                                rounds = 0;
                                crrRow++;
                            }
                            else rounds++;
                        }

                        crrRow++;
                        rounds = 0;
                        Container.RowDefinitions.Add(new RowDefinition());
                    }
                }

                callback?.Invoke(Container.Children[0]);
            }
        }



        private static TextBlock CerateLabel(string label)
        {
            return new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(6, 12, 6, -6),
                Foreground = Clr.Color("#78909C"),
                Text = label.ToUpper()
            };
        }



        private MidiaThumbCard CreateThumbCard(ProdutoType produto)
        {
            int itensPorProduto = ItensViewModel.CountItensPorProduto(produto.Id);
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            MidiaThumbCard midia = InstantiateThumbCard(produto, window, itensPorProduto);

            midia.SetMidia(produto.Image);

            midia.AddAction("Delete", () =>
            {
                window.DisplayDialog(new ConfirmDialog()
                        { Confirm = delegate
                            {
                                return ProdutosViewModel.Remove(produto.Id);
                            }
                        },
                        (View != null) ? View.LoadProdutos : null
                    );

                return true;
            });

            midia.SetMidiaAction((object sender) =>
            {
                window.DisplayDialog(new ImageDialog(((MidiaThumbCard)sender)._BorderMidia.Background));
                return true;
            });

            midia.SetHeaderAction((object sender) =>
            {
                window.DisplayDialog(new ProdutoDialog(produto));
                return true;
            });

            if (itensPorProduto > 0)
            {
                midia.SetSubHeaderAction((object sender) =>
                {
                    window.DisplayDialog(new ItensDialog(produto));
                    return true;
                });
            }

            return midia;
        }



        private MidiaThumbCard InstantiateThumbCard(ProdutoType produto, MainWindow window, int quantItens)
        {
            string subHeader = (Str.ZeroFill(quantItens) + " item".Pluralize(quantItens, "n"));

            return new MidiaThumbCard(this)
            {
                Margin = new Thickness(6, 12, 6, 0),
                Header = produto.Name,
                HeaderSize = 13,
                SubHeader = subHeader,
                SubHeaderSize = 12
            };
        }



        public void Clear()
        {
            if (Container != null)
            {
                Container.Children.Clear();
                Container.RowDefinitions.Clear();
                Container.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
