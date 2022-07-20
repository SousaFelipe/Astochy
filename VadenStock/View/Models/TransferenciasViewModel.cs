using System;
using System.Collections.Generic;
using System.Linq;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class TransferenciasViewModel
    {
        public static TransfType Last
        {
            get
            {
                TransfType[] select = AlmoxarifadoTransferencia.Model
                    .Select()
                    .OrderByDescending(a => a.Id)
                    .ToArray();

                return select[0];
            }
        }



        public static List<TransfType> TransfsPorItem(int item, params object[][] wheres)
        {
            AlmoxarifadoTransferencia model = AlmoxarifadoTransferencia.Model
                .Where("itens", "LIKE", $";{item};");

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
        }
    }
}
