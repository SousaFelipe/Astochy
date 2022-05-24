using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class MarcasViewModel
    {
        public static List<MarcaType> TodasAsMarcas
        {
            get { return Marca.Model.Select(); }
        }



        public static int Create(MarcaType marca)
        {
            List<string[]> inserts = new()
            {
                new string[] { "name", marca.Name },
                new string[] { "description", marca.Description }
            };

            return Marca.Model.Create(inserts);
        }
    }
}
