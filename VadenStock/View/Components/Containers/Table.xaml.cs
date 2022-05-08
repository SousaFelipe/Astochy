using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;



namespace VadenStock.View.Components.Containers
{
    public partial class Table : Border
    {
        public List<Row> Rows { get; private set; }
        public int Columns { get; private set; }



        public bool HasHeader;
        public Row? TableHeader;
        public RowDefinition? TableHeaderContent;



        public Options.TableOptions DefaultOptions = new();



        public Table()
        {
            Rows = new();
            Columns = 0;

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
            HasHeader = true;
            Columns = headers.Length;

            TableHeader = new(new()
            {
                RowSize = Options.RowOptions.Size.Large,
                Background = "#ECEFF1",
                FontStyle = FontStyles.Normal,
                FontWeight = FontWeights.Bold,
                Hover = false
            });

            Header? header;
            ColumnDefinition column;

            int  h;
            for (h = 0; h < headers.Length; h++)
            {
                header = headers[h];
                column = (header.W == Header.Width.Max)
                            ? new ColumnDefinition()
                            : new ColumnDefinition() { Width = GridLength.Auto };

                _GridContainer.ColumnDefinitions.Add(column);
                TableHeader.TD(header.Title);
            }

            Add(TableHeader);
        }



        public Table Add(Row row)
        {
            Rows.Add(row);
            return this;
        }



        public void Clear()
        {
            if (HasHeader)
            {
                TableHeader = Rows[0];
                
                try
                {
                    TableHeaderContent = _GridContainer.RowDefinitions[0];
                }
                catch (ArgumentOutOfRangeException)
                {
                    TableHeaderContent = new RowDefinition() { Height = GridLength.Auto };
                }
            }

            _GridContainer.Children.Clear();
            _GridContainer.RowDefinitions.Clear();

             Rows.Clear();

            if (HasHeader)
                Rows.Add(TableHeader);
        }



        public void DrawHeader()
        {
            if (TableHeader != null)
            {
                Border currentHeader;

                _GridContainer.RowDefinitions.Add(TableHeaderContent);

                int  i;
                for (i = 0; i < TableHeader.Borders.Count; i++)
                {
                    currentHeader = TableHeader.Borders[i];

                    _GridContainer.Children.Add(currentHeader);

                    Grid.SetColumn(currentHeader, i);
                    Grid.SetRow(currentHeader, 0);
                }

                HasHeader = true;
            }
        }



        public void Draw()
        {
            Row currentRow;
            Border currentData;

            DrawHeader();

            for (int r = 1; r <= DefaultOptions.DisplayRows; r++)
            {
                if (r == Rows.Count)
                    break;

                currentRow = Rows[r];
                
                if (currentRow != null)
                {
                    _GridContainer.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    currentRow.Paint((r % 2 != 0) ? null : "#F6FBFD");

                    for (int d = 0; d < Columns; d++)
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
}
