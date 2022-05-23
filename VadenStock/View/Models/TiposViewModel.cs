using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class TiposViewModel
    {
        public static int CountTiposPorCategoria(int categoria)
        {
            return Tipo.Model
                .Where("categoria", categoria.ToString())
                .Count();
        }

        public static List<TipoType> TiposPorCategoria(int categoria)
        {
            return Tipo.Model
                    .Where("categoria", categoria.ToString())
                    .Select();
        }
    }
}
