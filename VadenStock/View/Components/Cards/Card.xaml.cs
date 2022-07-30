using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using VadenStock.View.Components.Buttons;
using VadenStock.View.Components.Widgets;



namespace VadenStock.View.Components.Cards
{
    public partial class Card : Border
    {
        public enum ActionLevel
        {
            Simple,
            Success,
            Warning,
            Danger
        }



        private List<ButtonLight> Actions = new();



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

        

        public void Action(string icon, ActionLevel level, Action<object> action)
        {
            string color = (level == ActionLevel.Danger)
                ? "white"
                : "black";

            ButtonLight button = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 18,
                Height = 18,
                Margin = new Thickness(2),
                Content = Icon.Small(icon, GetActionColor(level))
            };

            button.Click += delegate
            {
                action?.Invoke(this);
            };

            Actions.Add(button);
            _StackActions.Children.Add(button);
        }



        public static string GetActionColor(ActionLevel level)
        {
            return level switch
            {
                ActionLevel.Simple => "gray",
                ActionLevel.Success => "green",
                ActionLevel.Warning => "black",
                ActionLevel.Danger => "red",
                _ => "blue"
            };
        }



        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                Focus();
            }
        }
    }
}
