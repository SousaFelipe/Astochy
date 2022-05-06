using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.View.Components.Containers;

using VadenStock.Tools;



namespace VadenStock.View.Components
{
    public partial class Pagination : UserControl
    {
        public Table? Table { get; set; }



        private int CurrentPageIndex = 0;
        private Row[]? Dataset = null;



        private readonly List<Row[]> Pages = new();
        private readonly List<Button> Controls = new();



        public Pagination()
        {
            InitializeComponent();
        }



        public void Paginate()
        {
            if (Table != null)
            {
                Dataset = Table.Rows.GetRange(1, Table.Rows.Count - 1).ToArray();
                System.Diagnostics.Trace.WriteLine($"Dataset Length [{ Dataset.Length }]");

                int tableRows = Table.DefaultOptions.DisplayRows + 1;
                decimal ceil = Math.Ceiling((decimal)(Dataset.Length / tableRows));
                int pages = (int)ceil;

                if ((ceil * tableRows) < Dataset.Length)
                    pages += 1;

                int start;
                int final;

                for (int i = 0; i < pages; i++)
                {
                    start = ((i * tableRows) - 1) < 0 ? 0 : (i * (tableRows - 1));
                    final = (tableRows * i) + (tableRows - (i + 2));

                    if (final > Dataset.Length - 1)
                        final -= (final - Dataset.Length);

                    Pages.Add(Dataset.Slice(start, final));

                    _StackControls.Children.Add(Control(i));

                    System.Diagnostics.Trace.WriteLine($"Start: [{ start }] Final[{ final }]");
                }

                Update();
            }
        }



        public void Update(int page = 0)
        {
            if (Table != null)
            {
                if (page < 0 || page >= Pages.Count)
                    return;

                Table.Clear();

                uint ui = 0;
                foreach (Row r in Pages[page])
                {
                    Table.Add(r);
                    string text = (r != null) ? r.GetHashCode().ToString() : "NULL";
                    System.Diagnostics.Trace.WriteLine($"{ ui } = { text }");
                    ui++;
                }

                Table.Draw();

                Select(page);
            }
        }



        private void Select(int page)
        {
            foreach (Button btn in Controls)
                btn.Style = (Style)FindResource("ButtonGray");

            Button button = Controls[page];
            button.Style = (Style)FindResource("ButtonSecondary");

            _ButtonPrevious.IsEnabled = (page > 0);
            _ButtonNext.IsEnabled = (page < (Pages.Count - 1));

            CurrentPageIndex = page;
        }



        private void Previous(object sender, RoutedEventArgs e)
        {
            int previous = CurrentPageIndex - 1;

            if (previous >= 0)
                Update(previous);
        }



        private void Next(object sender, RoutedEventArgs e)
        {
            int next = CurrentPageIndex + 1;

            if (next < Pages.Count)
                Update(next);
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
