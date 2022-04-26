using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Components
{
    public partial class ProdutoCard : UserControl
    {
        public ProdutoType Contract { get; private set; }



        public ProdutoCard(ProdutoType produto)
        {
            Contract = produto;

            InitializeComponent();

            Loaded += delegate
            {
                OnBeforeLoad();
            };
        }



        private void OnBeforeLoad()
        {
            int quantidade = Produto.New.Count().InnerJoin("items", "produto").Bind();

            _ImageAvatar.Source = Utils.FindStorageImage($"{ Contract.Image }-64.png");
            _TextName.Text = Contract.Name;
            _TextQuantItens.Text = Utils.ZeroFill(quantidade, " itens em estoque");
        }
    }
}
