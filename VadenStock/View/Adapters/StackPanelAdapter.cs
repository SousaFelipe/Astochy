using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using VadenStock.View.Components.Cards;



namespace VadenStock.View.Adapters
{
    public abstract class StackPanelAdapter
    {
        public StackPanel Container { get; private set; }



        protected StackPanelAdapter(StackPanel container)
        {
            Container = container;
        }



        public abstract void Build(Func<UIElement, bool>? callback = null);
    }
}
