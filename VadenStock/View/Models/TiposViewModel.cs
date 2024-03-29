﻿using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public static class TiposViewModel
    {
        public static List<TipoType> TodosOsTipos { get { return Tipo.Model.Select(); } }



        public static int Create(TipoStruct tipo)
        {
            List<object[]> inserts = new()
            {
                new object[] { "categoria", tipo.Categoria },
                new object[] { "name", tipo.Name },
                new object[] { "description", tipo.Description },
            };

            return Tipo.Model.Create(inserts);
        }



        public static int CountTiposPorCategoria(int categoria)
        {
            return Tipo.Model
                .Where("categoria", categoria)
                .Count();
        }



        public static List<TipoType> TiposPorCategoria(int categoria)
        {
            return (categoria > 0)
                ? Tipo.Model.Where("categoria", categoria).Select()
                : new(0);
        }
    }
}
