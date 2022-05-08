using System;
using System.Windows;
using System.Windows.Input;



namespace VadenStock.View.Components.Containers
{
    public class Options
    {
        public class TableOptions
        {
            public bool Stripped;
            public int DisplayRows;



            public TableOptions()
            {
                Stripped = false;
                DisplayRows = 10;
            }
        }



        public class RowOptions
        {
            public enum Size
            {
                Undefined = 0,
                Small = 1,
                Medium = 2,
                Large = 3
            }



            public Size RowSize;
            public TextAlignment Alignment;
            public string Background;
            public FontStyle FontStyle;
            public FontWeight FontWeight;
            public double FontSize;
            public Cursor? Cursor;
            public bool Divide;
            public bool Hover;



            public RowOptions()
            {
                RowSize = Size.Small;
                Alignment = TextAlignment.Left;
                Background = string.Empty;
                FontStyle = FontStyles.Normal;
                FontWeight = FontWeights.Normal;
                FontSize = 13;
                Cursor = Cursors.Arrow;
                Divide = false;
                Hover = true;
            }



            public static double GetSize(Size size)
            {
                return size switch
                {
                    Size.Small => 28,
                    Size.Medium => 32,
                    Size.Large => 40,
                    _ => 32
                };
            }
        }
    }
}
