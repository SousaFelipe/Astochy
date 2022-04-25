using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;
using VadenStock.View.Components;



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
                ProdutoCard currentCard;
                int rounds = 0;
                int crrRow = 0;

                for (int i = 0; i < Dataset.Count; i++)
                {
                    currentProduto = Dataset[i];

                    currentCard = new ProdutoCard(currentProduto)
                    {
                        Margin = new Thickness(6, (i > 0 && ((rounds - 3) == -3)) ? 12 : 0, 6, 0)
                    };

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
    }
}
