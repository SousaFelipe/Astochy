using System.Windows.Controls;

using VadenStock.View.Models;
using VadenStock.View.Adapters;



namespace VadenStock.View
{
    public partial class Produtos : UserControl
    {
        public Produtos()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadProdutos();
            };
        }



        public void LoadProdutos()
        {
            ProdutosAdapter adapter = new(_GridProdutos);

            adapter.Update(ProdutosViewModel.GetProdutos(), false);
            adapter.Build();
        }
    }
}
