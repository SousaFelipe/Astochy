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
        public Table? Table { get; set; }



        private Row? Header = null;
        private int CurrentPgi = 0;
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
                System.Diagnostics.Trace.WriteLine($"[Rows]: [{ Table.Rows.Count }] ");

                Header = Table.Rows[0];
                Dataset = Table.Rows.GetRange(1, Table.Rows.Count - 1).ToArray();

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
                        final -= (final - (Dataset.Length - 1));

                    Pages.Add(Dataset.Slice(start, final));

                    _StackControls.Children.Add(Control(i));
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
                Table.Add(Header);

                if (Header == null)
                    System.Diagnostics.Trace.WriteLine("Header é nulo!");

                foreach (Row r in Pages[page])
                    Table.Add(r);

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

            CurrentPgi = page;
        }



        private void Previous(object sender, RoutedEventArgs e)
        {
            int previous = CurrentPgi - 1;

            if (previous >= 0)
                Update(previous);
        }



        private void Next(object sender, RoutedEventArgs e)
        {
            int next = CurrentPgi + 1;

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
