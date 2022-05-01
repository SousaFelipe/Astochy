using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;
using VadenStock.View.Components;



namespace VadenStock.View.Adapters
{
    public class TabsAdapter : StackPanelAdapter
    {
        private List<CategoriaType> Dataset { get; set; }



        public TabsAdapter(StackPanel container) : base(container)
        {
            Dataset = new List<CategoriaType>();
        }



        public void Update(List<CategoriaType> dataset, bool buildAfterUpdate = true)
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
            if (Container != null)
            {
                Container.Children.Clear();

                foreach (CategoriaType item in Dataset)
                {
                    TabCategoria tab = new(this);
                    Container.Children.Add(tab.Inflate(item.Id));
                }

                callback?.Invoke(Container.Children[0]);
            }
        }
    }
}
