using System;
using System.Windows.Controls;



namespace VadenStock.View.Components.Forms
{
    public partial class SelectBox : ComboBox
    {
        public SelectBox()
        {
            InitializeComponent();
        }



        public ComboBoxItem? Find(object tag)
        {
            foreach(ComboBoxItem cbi in Items)
                if(cbi.Tag == tag)
                    return cbi;

            return null;
        }



        public int Clear(bool preserveFirst = false)
        {
            if (Items != null && Items.Count > 0)
            {
                if (preserveFirst)
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
