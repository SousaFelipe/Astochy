using System;
using System.Windows;
using System.Windows.Controls;



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
