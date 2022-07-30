using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Tools;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components;
using VadenStock.View.Components.Cards;



namespace VadenStock.View.Adapters
{
    public class OrcamentosAdapter : GridAdapter
    {
        public List<CompraType> Dataset = new();



        public OrcamentosAdapter(Grid container) : base(container) { }



        public void Update(List<CompraType> dataset, bool buildAfterUpdate = true)
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
                Container.RowDefinitions.Add(new RowDefinition());

                CompraType compra;
                Card card;

                int r = 0;
                int l = 0;

                for (int c = 0; c < Dataset.Count; c++)
                {
                    compra = Dataset[c];

                    card = CreateCard(
                            Str.Currency((compra.ValorTotal * 100).ToString()),
                            compra.Fornecedor.Fantasia,
                            (compra.DataEmissao ?? compra.CreatedDate).ToString("dd/MM/yyyy HH:mm").Replace(" ", " ás "),
                            new int[] { c, r }
                        );

                    Container.Children.Add(card);
                    
                    Grid.SetColumn(card, r);
                    Grid.SetRow(card, l);

                    if (0 == (r - 3))
                    {
                        Container.RowDefinitions.Add(new RowDefinition());

                        r = 0;
                        l++;
                    }
                    else r++;
                }

                callback?.Invoke(Container.Children[0]);
            }
        }



        public static Card CreateCard(string header, string sub, string body, int[] position)
        {
            Card card = new()
            {
                Margin = new Thickness(2, (position[0] > 0 && ((position[1] - 3) == -3)) ? 12 : 0, 2, 0)
            };

            card.Header(new Text(header, 18));
            card.SubHeader(new Text(sub, 13, Text.ColorSecondary));
            card.Body(new Text(body, 10, Text.ColorTertiary) { TextAlignment = TextAlignment.Right });

            card.Action("package-remove", Card.ActionLevel.Danger, sender =>
            {

            });

            card.Action("package-check", Card.ActionLevel.Success, sender =>
            {

            });

            return card;
        }
    }
}
