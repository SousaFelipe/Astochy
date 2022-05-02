using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Cards;

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
                Thickness thickn = new(6, (position > 0 && ((rounds - 3) == -3)) ? 12 : 0, 6, 0);
                string subHeader = Str.ZeroFill(ItensViewModel.CountItensPorProduto(produto.Id), " disponíveis");

                MidiaThumbCard midia = new()
                {
                    Margin = thickn,
                    Header = produto.Name,
                    HeaderSize = 13,
                    SubHeader = subHeader,
                    SubHeaderSize = 9
                };

                midia.SetMidia(produto.Image);

                return midia;
            }
        }
    }
}
