using System;
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
            List<object[]> inserts = new()
            {
                new object[] { "name", categoria.Name },
                new object[] { "description", categoria.Description }
            };

            return Categoria.Model.Create(inserts);
        }



        public static List<CategoriaType> Read(params object[][] wheres)
        {
            Categoria model = Categoria.Model;

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
        }
    }
}
