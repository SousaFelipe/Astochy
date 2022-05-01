using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class ProdutosViewModel
    {
        public static int Create(string name, int categoria, int tipo, int marca, string image, decimal price, string description)
        {
            List<string[]> inserts = new();

            inserts.Add(new string[] { "name", name });
            inserts.Add(new string[] { "categoria", categoria.ToString() });
            inserts.Add(new string[] { "tipo", tipo.ToString() });
            inserts.Add(new string[] { "marca", marca.ToString() });
            inserts.Add(new string[] { "image", image });
            inserts.Add(new string[] { "price", price.ToString() });
            inserts.Add(new string[] { "description", description });

            int output = Produto.New.Create(inserts);

            return output;
        }



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
