using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;
using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class DashboardViewModel
    {
        public static PatrimonioStruct GetPatrimonios()
        {
            int e = Item.New.Count().Where("localizacao", "Estoque").Bind();
            int c = Item.New.Count().Where("localizacao", "Comodato").Bind();
            int p = Item.New.Count().Where("localizacao", "Producao").Bind();

            PatrimonioStruct patrimonio = new()
            {
                Estoque = e,
                Comodato = c,
                Producao = p,
                Total = e + c + p
            };

            return patrimonio;
        }
    }
}
