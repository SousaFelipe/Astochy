using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Reflection;

using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Components;
using VadenStock.View.Components.Cards;
using VadenStock.View.Dialogs;


namespace VadenStock.View.Adapters
{
    public class OrcamentosAdapter : GridAdapter
    {
        public List<CompraType> Dataset = new();



        private object? View;



        public OrcamentosAdapter(Grid container) : base(container) { }



        public void SetView(object view)
        {
            View = view;
        }



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
                    card = CreateCard(compra, new int[] { c, r });

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



        public Card CreateCard(CompraType compra, int[] position)
        {
            Card card = new()
            {
                Margin = new Thickness(2, (position[0] > 0 && ((position[1] - 3) == -3)) ? 12 : 0, 2, 0)
            };

            string header = Str.Currency(compra.ValorTotal);
            string body = (compra.DataEmissao ?? compra.CreatedDate).ToString("dd/MM/yyyy HH:mm").Replace(" ", " ás ");

            card.Header(new Text(header, 18));
            card.SubHeader(new Text(compra.Fornecedor.Fantasia, 13, Text.ColorSecondary));
            card.Body(new Text(body, 10, Text.ColorTertiary) { TextAlignment = TextAlignment.Right });

            card.Action("open", Card.ActionLevel.Simple, sender =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                window.DisplayDialog(new OrcamentoDialog(compra), sender =>
                {
                    MethodInfo? loadOrc = View?.GetType().GetMethod("LoadOrcamentos");
                    MethodInfo? refComp = View?.GetType().GetMethod("RefreshCompras");
                    MethodInfo? refTabl = View?.GetType().GetMethod("RefreshTable");

                    loadOrc?.Invoke(View, null);
                    refComp?.Invoke(View, null);
                    refTabl?.Invoke(View, null);
                });
            });

            return card;
        }
    }
}
