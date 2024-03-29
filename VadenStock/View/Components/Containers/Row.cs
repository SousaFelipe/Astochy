﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using VadenStock.View.Components.Buttons;



namespace VadenStock.View.Components.Containers
{
    public class Row
    {
        public enum ActionLevel
        {
            None,
            Info,
            Warning,
            Danger
        }



        public uint Count { get; set; }
        public List<Border> Borders { get; private set; }
        public Action<object>? Callback { get; private set; }



        public Options.RowOptions DefaultOptions = new();



        private SolidColorBrush HoverBrush;



        public Row(Options.RowOptions? options = null)
        {
            FilterOptions(options);

            Count = 0;
            Borders = new();
            Callback = null;
            HoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECEFF3"));
        }



        private void FilterOptions(Options.RowOptions? options)
        {
            if (options != null)
            {
                DefaultOptions.RowSize = (options.RowSize == Options.RowOptions.Size.Undefined) ? DefaultOptions.RowSize : options.RowSize;
                DefaultOptions.Alignment = options.Alignment;
                DefaultOptions.Background = options.Background ?? DefaultOptions.Background;
                DefaultOptions.FontStyle = options.FontStyle;
                DefaultOptions.FontWeight = options.FontWeight;
                DefaultOptions.FontSize = options.FontSize > 8 ? options.FontSize : DefaultOptions.FontSize;
                DefaultOptions.Cursor = options.Cursor ?? DefaultOptions.Cursor;
                DefaultOptions.Divide = options.Divide;
                DefaultOptions.Hover = options.Hover;
            }
        }



        public Border Get(int position)
        {
            return position < Borders.Count ? Borders[position] : new Border();
        }



        public void Paint(string? color)
        {
            if (!string.IsNullOrEmpty(color))
            {
                foreach (Border b in Borders)
                    b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
        }



        public void SetHover(string color)
        {
            HoverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }



        private TextBlock Content(object data, Options.RowOptions options)
        {
            Options.RowOptions crrOptions = options ?? DefaultOptions;

            return new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = crrOptions.Alignment,

                Margin = new Thickness(
                            Count == 0 ? 16 : 8, 0,
                            Count == Borders.Count ? 16 : 8, 0
                        ),

                FontSize = crrOptions.FontSize,
                FontStyle = crrOptions.FontStyle,
                FontWeight = crrOptions.FontWeight,
                Text = (data != null) ? data.ToString() : string.Empty
            };
        }



        private static Border Container(UIElement child, Options.RowOptions options)
        {
            return new Border()
            {
                Height = Options.RowOptions.GetSize(options.RowSize),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = string.IsNullOrEmpty(options.Background)
                    ? null
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString(options.Background)),
                Cursor = options.Cursor,
                BorderBrush = options.Divide ? new SolidColorBrush(Colors.LightGray) : null,
                BorderThickness = new Thickness(0, 0, 0, options.Divide ? 1 : 0),
                Child = child
            };
        }



        public Row AC(object data, ActionLevel level = ActionLevel.None, Action<object>? callback = null, Options.RowOptions? options = null)
        {
            Callback = callback;
            Options.RowOptions crrOptions = options ?? DefaultOptions;

            var button = (Button)(
                (level == ActionLevel.Danger)
                    ? new ButtonDanger()
                    : (level == ActionLevel.Warning)
                        ? new ButtonWarning()
                        : (level == ActionLevel.Info)
                            ? new ButtonGray()
                            : new ButtonLight()
            );

            button.Content = data;
            button.Click += EventCallback;

            Border container = Container(button, crrOptions);
            Borders.Add(container);

            Count++;

            return this;
        }



        public Row TD(object data, Options.RowOptions? options = null)
        {
            Options.RowOptions crrOptions = options ?? DefaultOptions;
            TextBlock content = Content(data, crrOptions);
            Border container = Container(content, crrOptions);

            Borders.Add(container);

            if (crrOptions.Hover)
            {
                Brush brush = new SolidColorBrush(Colors.Transparent);

                container.MouseEnter += delegate
                {
                    brush = Borders[0].Background;

                    foreach (Border b in Borders)
                        b.Background = HoverBrush;
                };

                container.MouseLeave += delegate
                {
                    foreach (Border b in Borders)
                        b.Background = brush;
                };
            }

            Count++;

            return this;
        }



        private void EventCallback(object sender, RoutedEventArgs e)
        {
            if (Callback != null)
                Callback.Invoke(sender);
        }
    }
}
