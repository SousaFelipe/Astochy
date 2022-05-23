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
    }
}
