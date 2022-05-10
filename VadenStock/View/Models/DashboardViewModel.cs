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
            int estoque = Item.New.Count().Where("localizacao", "Estoque").Bind();
            int comodato = Item.New.Count().Where("localizacao", "Comodato").Bind();
            int rota = Item.New.Count().Where("localizacao", "Em Rota").Bind();

            PatrimonioStruct patrimonio = new()
            {
                Estoque = estoque,
                Comodato = comodato,
                EmRota = rota,
                Total = estoque + comodato + rota
            };

            return patrimonio;
        }
    }
}
