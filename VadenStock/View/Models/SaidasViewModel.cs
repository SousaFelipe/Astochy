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
            List<string[]> inserts = new()
            {
                new string[] { "transferencia", (saida.Transferencia ?? new TransfType()).Id.ToString() },
                new string[] { "responsavel", saida.Responsavel.ToString() },
                new string[] { "tipo", ItemType.GetStatusName(saida.Tipo) }
            };

            return Saida.Model.Create(inserts);
        }
    }
}
