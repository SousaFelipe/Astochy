using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class ProdutosViewModel
    {
        public static List<ProdutoType> TodosOsProdutos
        {
            get { return Produto.New.Select().Get(); }
        }



        public static int CountTodosOsProdutos
        {
            get { return Produto.New.Count().Bind(); }
        }



        public static int Create(ProdutoStruct produto)
        {
            List<string[]> inserts = new();

            inserts.Add(new string[] { "name", produto.Name });
            inserts.Add(new string[] { "categoria", produto.Categoria.ToString() });
            inserts.Add(new string[] { "tipo", produto.Tipo.ToString() });
            inserts.Add(new string[] { "marca", produto.Marca.ToString() });
            inserts.Add(new string[] { "image", $"{ produto.Image.FileName }{ produto.Image.FileExtension }" });
            inserts.Add(new string[] { "price", produto.Price.ToString().Replace(".", "").Replace(",", ".") });
            inserts.Add(new string[] { "description", produto.Description });

            int output = Produto.New.Create(inserts);

            return output;
        }



        public static List<ProdutoType> FiltrarProdutos(ProdutoStruct produto)
        {
            Produto model = Produto.New.Select();

            if (produto.Categoria > 0)
                model.Where("categoria", produto.Categoria.ToString());

            if (produto.Tipo > 0)
                model.Where("tipo", produto.Tipo.ToString());

            if (produto.Marca > 0)
                model.Where("marca", produto.Marca.ToString());

            return model.Get();
        }
    }
}
