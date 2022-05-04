using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;



namespace VadenStock.View.Components.Containers
{
    public partial class Table : Border
    {
        public List<Row> Rows { get; private set; }



        public int DISPLAY_ROWS = 10;



        public Table()
        {
            Rows = new();

            InitializeComponent();
        }



        public void Headers(params string[] headers)
        {
            Row row = new(new Row.Options()
            {
                Size = Row.Size.Medium,
                FontStyle = FontStyles.Normal,
            });

            uint h;
            for (h = 0; h < headers.Length; h++)
            {
                row.TD(headers[h]);

                _GridContainer.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Add(row);
        }



        public Table Add(Row row)
        {
            Rows.Add(row);
            return this;
        }



        public void Draw()
        {
            Row currentRow;
            Border currentData;

            int d;
            int r;

            for (r = 0; r < DISPLAY_ROWS + 1; r++)
            {
                if (r == Rows.Count)
                    break;

                _GridContainer.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                currentRow = Rows[r];

                for (d = 0; d < _GridContainer.ColumnDefinitions.Count; d++)
                {
                    currentData = currentRow.Get(d);

                    _GridContainer.Children.Add(currentData);

                     Grid.SetColumn(currentData, d);
                     Grid.SetRow(currentData, r);
                }
            }
        } 
    }
}
