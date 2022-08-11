using System;
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



        public static int Create(ProdutoStruct produto)
        {
            List<object[]> inserts = new()
            {
                new object[] { "name", produto.Name },
                new object[] { "categoria", produto.Categoria },
                new object[] { "tipo", produto.Tipo },
                new object[] { "marca", produto.Marca },
                new object[] { "image", $"{ produto.Image.FileName }{ produto.Image.FileExtension }" },
                new object[] { "valor", produto.Valor },
                new object[] { "description", produto.Description }
            };

            return Produto.Model.Create(inserts);
        }



        public static ProdutoType? Find(int id)
        {
            List<ProdutoType> list = Produto.Model
                .Where("id", id)
                .Select();

            return list.Count > 0
                ? list[0]
                : null;
        }



        public static List<ProdutoType> Read(params object[][] wheres)
        {
            Produto model = Produto.Model;

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
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
            Dictionary<string, List<ProdutoType>> output = new();

            List<MarcaType> marcas = MarcasViewModel.TodasAsMarcas;
            List<ProdutoType> ordered;

            foreach (MarcaType marca in marcas)
            {
                ordered = new();

                foreach (ProdutoType produto in produtos)
                    if (produto.Marca.Id == marca.Id)
                        ordered.Add(produto);

                output.Add(marca.Name, ordered);
            }

            return output;
        }



        public static bool Remove(int produto)
        {
            return Produto.Model.Delete(produto);
        }
    }
}
