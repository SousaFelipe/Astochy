using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class ProdutosViewModel
    {
        public static List<ProdutoType> GetProdutos()
        {
            return Produto.New.Select().Get();
        }
    }
}
