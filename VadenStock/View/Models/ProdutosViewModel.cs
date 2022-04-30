using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class ProdutosViewModel
    {
        public static List<ProdutoType> GetProdutos(ProdutoFilter filter)
        {
            Produto model = Produto.New.Select();

            if (filter.Categoria > 0)
                model.Where("categoria", filter.Categoria.ToString());

            if (filter.Tipo > 0)
                model.Where("tipo", filter.Tipo.ToString());

            if (filter.Marca > 0)
                model.Where("marca", filter.Marca.ToString());

            return model.Get();
        }
    }
}
