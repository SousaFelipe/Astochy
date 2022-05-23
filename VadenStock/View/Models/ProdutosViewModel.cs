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
            get { return Produto.Model.Select(); }
        }



        public static int CountTodosOsProdutos
        {
            get { return Produto.Model.Count(); }
        }



        public static int Create(ProdutoStruct produto)
        {
            List<string[]> inserts = new()
            {
                new string[] { "name", produto.Name },
                new string[] { "categoria", produto.Categoria.ToString() },
                new string[] { "tipo", produto.Tipo.ToString() },
                new string[] { "marca", produto.Marca.ToString() },
                new string[] { "image", $"{ produto.Image.FileName }{ produto.Image.FileExtension }" },
                new string[] { "price", produto.Price.ToString().Replace(".", "").Replace(",", ".") },
                new string[] { "description", produto.Description }
            };

            int output = Produto.Model.Create(inserts);

            return output;
        }



        public static bool Remove(int produto)
        {
            return Produto.Model.Delete(produto);
        }



        public static List<ProdutoType> ProdutosPorMarca(int marca)
        {
            return Produto.Model
                .Where("marca", marca.ToString())
                .Select();
        }



        public static List<ProdutoType> ProdutosPorCategoria(int categoria)
        {
            return Produto.Model
                .Where("categoria", categoria.ToString())
                .Select();
        }



        public static List<ProdutoType> ProdutosPorTipo(int tipo)
        {
            return Produto.Model
                .Where("tipo", tipo.ToString())
                .Select();
        }



        public static List<ProdutoType> FilterData(ProdutoStruct produto)
        {
            Produto model = Produto.Model;

            if (produto.Categoria > 0)
                model.Where("categoria", produto.Categoria.ToString());

            if (produto.Tipo > 0)
                model.Where("tipo", produto.Tipo.ToString());

            if (produto.Marca > 0)
                model.Where("marca", produto.Marca.ToString());

            return model.Select();
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
