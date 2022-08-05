using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class SaidasViewModel
    {
        public static int Create(SaidaType saida)
        {
            List<object[]> inserts = new()
            {
                new object[] { "transferencia", (saida.Transferencia ?? new TransfType()).Id },
                new object[] { "responsavel", saida.Responsavel },
                new object[] { "tipo", ItemType.GetStatusName(saida.Tipo) }
            };

            return Saida.Model.Create(inserts);
        }
    }
}
