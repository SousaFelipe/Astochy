using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.View.Components.Containers;
using VadenStock.View.Components.Buttons;

using VadenStock.Tools;



namespace VadenStock.View.Components
{
    public partial class Pagination : UserControl
    {
        public static readonly DependencyProperty TableProperty = DependencyProperty.Register(
                "Table",
                typeof(Table),
                typeof(Pagination),
                new UIPropertyMetadata(null, TablePropertyCallback)
            );

        public Table Table
        {
            get { return (Table)GetValue(TableProperty); }
            set { SetValue(TableProperty, value); }
        }

        public static void TablePropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            Pagination pagination = (Pagination)root;
            pagination.Table = (Table)e.NewValue;
        }



        private List<Row> Dataset = new();

        private List<Button> Controls = new();

        private Dictionary<int, List<Row>> Pages = new();



        public Pagination()
        {
            InitializeComponent();
        }



        public void Update()
        {
            Dataset = Table.Rows.GetRange(1, Table.Rows.Count - 1);

            int tableRows = Table.DefaultOptions.DisplayRows;
            decimal ceiling = Math.Ceiling((decimal)((Dataset.Count - 1) / tableRows));
            int pages = Convert.ToInt32(ceiling) + (ceiling * tableRows) < (Dataset.Count - 1) ? 1 : 0;

            int start;
            int final;

            for (int i = 0; i < pages; i++)
            {
                start = (i * tableRows);
                final = (tableRows * i) + (tableRows - 1);

                if (final > (Dataset.Count - 1))
                    final -= (final - (Dataset.Count - 1));

                Pages.Add(i, Dataset.GetRange(start, final));

                _StackControls.Children.Add( Control(i) );
            }

            Table.Rows = Pages[0];
            Table.Draw();
        }



        public void Select(int page)
        {
            Button button = Controls[page];
            button.Style = (Style)FindResource("ButtonSecondary");
        }



        public void Previous()
        {

        }



        public void Next()
        {

        }



        public Button Control(int page)
        {
            Button button = new()
            {
                Height = 28,
                Content = Str.ZeroFill(page + 1),
                Style = (Style)FindResource(page > 0 ? "ButtonGray" : "ButtonSecondary")
            };

            Controls.Add(button);

            return button;
        }
    }
}
