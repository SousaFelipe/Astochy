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
        public enum Size
        {
            Undefined = 0,
            Small = 1,
            Medium = 2,
            Large = 3
        }



        public class Options
        {
            public Size Size;
            public TextAlignment Alignment;
            public FontStyle FontStyle;
            public double FontSize;
            public Cursor? Cursor;
            public bool Hover;

            public static double GetSize(Size size)
            {
                return size switch
                {
                    Size.Small => 28,
                    Size.Medium => 32,
                    Size.Large => 42,
                    _ => 32
                };
            }
        }



        public uint Count { get; set; }
        public List<Border> Borders { get; private set; }



        public Options RowOptions = new()
        {
            Size = Size.Small,
            Alignment = TextAlignment.Left,
            FontStyle = FontStyles.Normal,
            FontSize = 13,
            Cursor = Cursors.Arrow,
        };



        public Row(Options? options = null)
        {
            if (options != null)
            {
                RowOptions.Size = (options.Size == Size.Undefined) ? RowOptions.Size : options.Size;
                RowOptions.Alignment = options.Alignment;
                RowOptions.FontStyle = options.FontStyle;
                RowOptions.FontSize = options.FontSize > 8 ? options.FontSize : RowOptions.FontSize;
                RowOptions.Cursor = options.Cursor ?? RowOptions.Cursor;
            }

            Count = 0;
            Borders = new();
        }



        public Border Get(int position)
        {
            return position < Borders.Count ? Borders[position] : new Border();
        }



        public Row TD(object data, Options? options = null)
        {
            Options crrOptions = options ?? RowOptions;

            Border border = new()
            {
                Height = Options.GetSize(crrOptions.Size),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Cursor = crrOptions.Cursor,

                Child = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextAlignment = crrOptions.Alignment,

                    Margin = new Thickness(
                            Count == 0 ? 8 : 4, 0,
                            Count == Borders.Count ? 8 : 4, 0
                        ),

                    FontSize = crrOptions.FontSize,
                    FontStyle = crrOptions.FontStyle,
                    Text = data.ToString()
                }
            };

            Borders.Add(border);

            if (crrOptions.Hover)
            {
                border.MouseEnter += delegate
                {
                    foreach (Border b in Borders)
                        b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E7F0F6"));
                };

                border.MouseLeave += delegate
                {
                    foreach (Border b in Borders)
                        b.Background = null;
                };
            }

            Count++;

            return this;
        }
    }
}
