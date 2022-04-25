using System.Windows.Controls;

using VadenStock.Model.Types;



namespace VadenStock.View.Components
{
    public partial class ProdutoCard : UserControl
    {
        public ProdutoType Produto { get; private set; }



        public ProdutoCard(ProdutoType produto)
        {
            Produto = produto;

            InitializeComponent();

            Loaded += delegate
            {
                _TextName.Text = produto.Name;
            };
        }
    }
}
