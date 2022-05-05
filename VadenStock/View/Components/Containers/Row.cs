using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;



namespace VadenStock.View.Components.Containers
{
    public class Row
    {
        public uint Count { get; set; }
        public List<Border> Borders { get; private set; }



        public Options.RowOptions DefaultOptions = new();



        public Row(Options.RowOptions? options = null)
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
                DefaultOptions.Hover = options.Hover;
            }

            Count = 0;
            Borders = new();
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



        public TextBlock Content(object data, Options.RowOptions options)
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
                Text = data.ToString()
            };
        }



        public static Border Container(UIElement child, Options.RowOptions options)
        {
            return new Border()
            {
                Height = Options.RowOptions.GetSize(options.RowSize),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = string.IsNullOrEmpty(options.Background) ? null : new SolidColorBrush((Color)ColorConverter.ConvertFromString(options.Background)),
                Cursor = options.Cursor,
                Child = child
            };
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
                        b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECEFF3"));
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
    }
}
