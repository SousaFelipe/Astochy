using System;
using System.Windows;
using System.Reflection;
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


        private int Columns;
        private object? View;



        public ProdutosAdapter(Grid container) : base(container)
        {
            Columns = 4;
        }



        public void SetColumns(int columns)
        {
            Columns = columns;
        }



        public void SetView(object view)
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
                int row = 0;

                foreach (string key in OrderedDataset.Keys)
                {
                    if (OrderedDataset[key] != null && OrderedDataset[key].Count > 0)
                    {
                        currentDataset = OrderedDataset[key];
                        currentLabel = CreateLabel(key);

                        Container.Children.Add(currentLabel);

                        Grid.SetRow(currentLabel, row);
                        Grid.SetColumn(currentLabel, 0);
                        Grid.SetColumnSpan(currentLabel, Columns);

                        row++;

                        for (int i = 0; i < currentDataset.Count; i++)
                        {
                            currentProduto = currentDataset[i];
                            currentCard = CreateThumbCard(currentProduto);

                            Container.Children.Add(currentCard);

                            Grid.SetColumn(currentCard, rounds);
                            Grid.SetRow(currentCard, row);

                            if ((rounds - (Columns - 1)) == 0 || i == (currentDataset.Count - 1))
                            {
                                Container.RowDefinitions.Add(new RowDefinition());

                                rounds = 0;
                                row++;
                            }
                            else
                                rounds++;
                        }

                        row++;
                        rounds = 0;

                        Container.RowDefinitions.Add(new RowDefinition());
                    }
                }

                callback?.Invoke(Container.Children[0]);
            }
        }



        private static TextBlock CreateLabel(string label)
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
            MidiaThumbCard midia = InstantiateThumbCard(produto, itensPorProduto);

            midia.SetMidia(produto.Image);
            midia.AddAction("Delete", () =>
            {
                MethodInfo? method = View?.GetType().GetMethod("LoadProdutos");

                window.DisplayDialog
                (
                    new ConfirmDialog(ConfirmDialog.ConfirmType.Delete, "Tem certeza que você deseja remover esse registro?").OnConfirm((sender) => ProdutosViewModel.Remove(produto.Id)),
                    sender => { method?.Invoke(View, null); }
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



        private MidiaThumbCard InstantiateThumbCard(ProdutoType produto, int quantItens)
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
    }
}
