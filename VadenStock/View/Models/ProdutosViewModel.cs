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



        public static Dictionary<string, List<ProdutoType>> OrderByMarca(List<ProdutoType> produtos)
        {
            List<ProdutoType> typeToOrder;
            Dictionary<string, List<ProdutoType>> ordered = new();
            List<MarcaType> marcas = MarcasViewModel.TodasAsMarcas;

            foreach (MarcaType mar in marcas)
            {
                typeToOrder = new();

                foreach (ProdutoType pro in produtos)
                    if (pro.Marca.Id == mar.Id)
                        typeToOrder.Add(pro);

                ordered.Add(mar.Name, typeToOrder);
            }

            return ordered;
        }



        public static Dictionary<string, List<ProdutoType>> OrderByCategoria(List<ProdutoType> produtos)
        {
            List<ProdutoType> typeToOrder;
            Dictionary<string, List<ProdutoType>> ordered = new();
            List<CategoriaType> categorias = CategoriasViewModel.TodasAsCategorias;
            
            foreach (CategoriaType cat in categorias)
            {
                typeToOrder = new();

                foreach (ProdutoType pro in produtos)
                    if (pro.Categoria.Id == cat.Id)
                        typeToOrder.Add(pro);

                ordered.Add(cat.Name, typeToOrder);
            }

            return ordered;
        }
    }
}
