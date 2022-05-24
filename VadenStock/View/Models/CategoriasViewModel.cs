using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class CategoriasViewModel
    {
        public static List<CategoriaType> TodasAsCategorias
        {
            get { return Categoria.Model.Select(); }
        }

        public static int CountTodasAsCategorias
        {
            get { return Categoria.Model.Count(); }
        }



        public static int Create(CategoriaType categoria)
        {
            List<string[]> inserts = new()
            {
                new string[] { "name", categoria.Name },
                new string[] { "description", categoria.Description }
            };

            return Categoria.Model.Create(inserts);
        }
    }
}
