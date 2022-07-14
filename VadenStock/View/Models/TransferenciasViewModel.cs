using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class TransferenciasViewModel
    {
        public static List<AlmoxTransfType> TransfsPorItem(int item, params object[][] query)
        {
            AlmoxarifadoTransferencia model = AlmoxarifadoTransferencia.Model
                .Where("itens", "LIKE", $";{item};");

            foreach (object[] q in query)
                model.Where(Convert.ToString(q[0]), Convert.ToString(q[1]));

            return model.Select();
        }
    }
}
