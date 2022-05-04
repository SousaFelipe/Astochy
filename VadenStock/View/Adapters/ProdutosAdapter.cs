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



        public ProdutosAdapter(Grid container) : base(container) { }



        public void Update(List<ProdutoType> dataset, bool buildAfterUpdate = true)
        {
            if (Dataset == null)
                Dataset = dataset;
            else
                Dataset.AddRange(dataset);

            if (buildAfterUpdate)
                Build();
        }



        public override void Build(Func<UIElement, bool>? callback = null)
        {
            Clear();

            if (Container != null)
            {
                ProdutoType currentProduto;
                MidiaThumbCard currentCard;

                int rounds = 0;
                int crrRow = 0;

                for (int i = 0; i < Dataset.Count; i++)
                {
                    currentProduto = Dataset[i];
                    currentCard = Molecules.ProdutoThumbCard(currentProduto, i, rounds);

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

                callback?.Invoke(Container.Children[0]);
            }
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



        private static class Molecules
        {
            public static MidiaThumbCard ProdutoThumbCard(ProdutoType produto, int position, int rounds)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                Thickness thickn = new(6, (position > 0 && ((rounds - 3) == -3)) ? 12 : 0, 6, 0);
                List<ItemType> itens = ItensViewModel.ItensPorProduto(produto.Id);
                string subHeader = Str.ZeroFill(itens.Count, " disponíveis");

                MidiaThumbCard midia = new()
                {
                    Margin = thickn,
                    Header = produto.Name,
                    HeaderSize = 13,
                    SubHeader = subHeader,
                    SubHeaderSize = 9
                };

                midia.SetMidia(produto.Image);

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

                //if (itens.Count > 0)
                //{
                    midia.SetSubHeaderAction((object sender) =>
                    {
                        window.DisplayDialog(new ItensDialog());
                        return true;
                    });
                //}

                return midia;
            }
        }
    }
}
