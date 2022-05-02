using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class MarcasViewModel
    {
        public static List<MarcaType> TodasAsMarcas
        {
            get { return Marca.New.Select().Get(); }
        }
    }
}
