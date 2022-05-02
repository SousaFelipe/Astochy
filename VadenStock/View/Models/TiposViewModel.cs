using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class TiposViewModel
    {
        public static int CountTiposPorCategoria(int categoria)
        {
            return Tipo.New.Count()
                .Where("categoria", categoria.ToString())
                .Bind();
        }

        public static List<TipoType> TiposPorCategoria(int categoria)
        {
            return Tipo.New.Select()
                    .Where("categoria", categoria.ToString())
                    .Get();
        }
    }
}
