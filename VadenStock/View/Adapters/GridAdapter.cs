using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using VadenStock.View.Components.Cards;



namespace VadenStock.View.Adapters
{
    public abstract class GridAdapter
    {
        public Grid Container { get; private set; }



        public GridAdapter(Grid container)
        {
            Container = container;
        }



        public abstract void Build(Func<UIElement, bool>? callback = null);
    }
}
