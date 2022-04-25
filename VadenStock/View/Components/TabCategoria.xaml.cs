using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model;
using VadenStock.Model.Types;
using VadenStock.View.Adapters;



namespace VadenStock.View.Components
{
    public partial class TabCategoria : RadioButton
    {
        public CategoriaType? Item { get; private set; }



        private TabsAdapter Adapter { get; set;  }



        public TabCategoria(TabsAdapter adapter)
        {
            InitializeComponent();

            Adapter = adapter;
        }



        public TabCategoria Inflate(int id)
        {
            Item = Categoria.New.Get(id)[0];

            if (Item != null)
                _radioControl.Content = Item.Value.Name;

            return this;
        }
    }
}
