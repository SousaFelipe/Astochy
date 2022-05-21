using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components.Cards
{
    public partial class Card : Border
    {
        public Card()
        {
            InitializeComponent();
        }



        public Card Header(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                _GridHeader.Children.Add(element);
            
            return this;
        }



        public Card SubHeader(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                _GridSubHeader.Children.Add(element);
            
            return this;
        }



        public Card Body(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                _GridBody.Children.Add(element);
            
            return this;
        }
    }
}
