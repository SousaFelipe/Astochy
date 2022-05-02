using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class AlmoxarifadosViewModel
    {
        public static List<AlmoxType> TodosOsAlmoxarifados
        {
            get { return Almoxarifado.New.Select().Get(); }
        }
    }
}
