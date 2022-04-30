using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class CategoriasViewModel
    {
        public static List<CategoriaType> GetCategorias()
        {
            return Categoria.New.Select().Get();
        }
    }
}
