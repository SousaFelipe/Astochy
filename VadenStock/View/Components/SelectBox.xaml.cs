using System;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components
{
    public partial class SelectBox : ComboBox
    {
        public SelectBox()
        {
            InitializeComponent();
        }



        public int Clear(bool safe = false)
        {
            if (Items != null && Items.Count > 0)
            {
                if (safe)
                {
                    ComboBoxItem first = (ComboBoxItem)Items[0];

                    Items.Clear();
                    first.IsSelected = true;
                    Items.Add(first);

                    return 1;
                }

                Items.Clear();

                return 0;
            }

            return -1;
        }
    }
}
