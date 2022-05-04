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
        public List<Row> Rows { get; set; }



        public Options.TableOptions DefaultOptions = new();



        public Table()
        {
            Rows = new();

            InitializeComponent();
        }



        public Table(Options.TableOptions? options = null)
        {
            if (options != null)
            {
                DefaultOptions.Stripped = options.Stripped;
                DefaultOptions.DisplayRows = options.DisplayRows > 0 ? options.DisplayRows : DefaultOptions.DisplayRows;
            }

            Rows = new();

            InitializeComponent();
        }



        public void Headers(params Header[] headers)
        {
            Row row = new(new()
            {
                RowSize = Options.RowOptions.Size.Large,
                Background = "#ECEFF1",
                FontStyle = FontStyles.Normal,
                FontWeight = FontWeights.Bold,
                Hover = false
            });

            Header current;
            ColumnDefinition column;

            uint h;
            for (h = 0; h < headers.Length; h++)
            {
                current = headers[h];

                column = (current.W == Header.Width.Max)
                    ? new ColumnDefinition()
                    : new ColumnDefinition() { Width = GridLength.Auto };

                row.TD(current.Title);

                _GridContainer.ColumnDefinitions.Add(column);
            }

            Add(row);
        }



        public Table Add(Row row)
        {
            Rows.Add(row);
            return this;
        }



        public void Divide(int row, int headers)
        {
            Border div = new()
            {
                Height = 1,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.LightGray)
            };

            _GridContainer.Children.Add(div);

            Grid.SetRow(div, row);
            Grid.SetColumnSpan(div, headers);
        }



        public void Draw()
        {
            Row currentRow;
            Border currentData;

            int columns = _GridContainer.ColumnDefinitions.Count;

            int d;
            int r;

            for (r = 0; r < DefaultOptions.DisplayRows + 1; r++)
            {
                if (r == Rows.Count)
                    break;

                currentRow = Rows[r];
                
                if (DefaultOptions.Stripped)
                    currentRow.Paint((r % 2 != 0) ? null : "#F6FBFD");

                _GridContainer.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                for (d = 0; d < columns; d++)
                {
                    currentData = currentRow.Get(d);

                    _GridContainer.Children.Add(currentData);

                     Grid.SetColumn(currentData, d);
                     Grid.SetRow(currentData, r);
                }
            }

            Divide(0, columns);
        } 
    }
}
