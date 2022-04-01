using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model;
using VadenStock.View.Adapters;



namespace VadenStock.View.Components
{
    public partial class TabCategoria : RadioButton
    {
        public Categoria.Contract? Item { get; private set; }



        private TabsAdapter Adapter { get; set;  }



        public TabCategoria(TabsAdapter adapter)
        {
            InitializeComponent();

            Adapter = adapter;
        }



        public TabCategoria Inflate(int id)
        {
            Item = Categoria.New.Load(id);

            if (Item != null)
                _radioControl.Content = Item.Value.Name;

            return this;
        }



        private void CallOnTabChange(object sender, RoutedEventArgs e)
        {
            Adapter.ChangeTab(this);
        }
    }
}
