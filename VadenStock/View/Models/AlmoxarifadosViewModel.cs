using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class AlmoxarifadosViewModel
    {
        public static List<AlmoxType> TodosOsAlmoxarifados
        {
            get { return Almoxarifado.Model.Select(); }
        }



        public static AlmoxType? Find(int id)
        {
            List<AlmoxType> search = Almoxarifado.Model
                .Where("id", id.ToString())
                .Select();

            return search.Count > 0
                ? search[0]
                : null;
        }
    }
}
