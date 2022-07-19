using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;



namespace VadenStock.View.Components.Containers
{
    public partial class ListItem : Grid
    {
        public int Id { get; private set; }
        public Func<bool>? Callback { get; private set; } 



        public ListItem()
        {
            InitializeComponent();
        }



        public ListItem(int id, params string[] children)
        {
            Id = id;

            InitializeComponent();

            Loaded += delegate
            {
                InitComponent();
                LoadElements(children);
                LoadInteractions();
            };
        }



        private void InitComponent()
        {
            Cursor = Cursors.Hand;
            Background = new SolidColorBrush(Colors.White);
        }



        private void LoadElements(string[] children)
        {
            string param;
            string value;
            TextBlock text;

            ColumnDefinitions.Clear();

            for (int i = 0; i < children.Length; i++)
            {
                param = children[i].Split(":")[0];
                value = children[i].Split(":")[1];

                ColumnDefinitions.Add(Definition(param));
                text = Content(value);
                Children.Add(text);
                SetColumn(text, i);
            }
        }



        private void LoadInteractions()
        {
            MouseEnter += (sender, e) =>
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECEFF1"));
            };

            MouseLeave += (sender, e) =>
            {
                Background = new SolidColorBrush(Colors.White);
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                Callback?.Invoke();
            };
        }



        public ListItem Action(Func<bool> callback)
        {
            Callback = callback;
            return this;
        }



        private TextBlock Content(string text)
        {
            int left = (Children.Count > 0) ? 4 : 8;

            return new TextBlock()
            {
                Margin = new Thickness(left, 0, 4, 0),
                FontSize = 14,
                Text = text
            };
        }



        private static ColumnDefinition Definition(string param)
        {
            ColumnDefinition def = new();

            if (param == "auto")
                def.Width = GridLength.Auto;

            return def;
        }
    }
}
