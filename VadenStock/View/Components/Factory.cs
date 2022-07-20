using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components
{
    public class Factory
    {
        public static StackPanel Stack(params UIElement[] childrens)
        {
            StackPanel stack = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            foreach (UIElement child in childrens)
                stack.Children.Add(child);

            return stack;
        }
    }
}
