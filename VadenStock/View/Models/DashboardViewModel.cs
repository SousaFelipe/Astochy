using VadenStock.Model;
using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class DashboardViewModel
    {
        public static PatrimonioStruct GetPatrimonios()
        {
            int estoque = Item.Model.Where("localizacao", "Estoque").Count();
            int comodato = Item.Model.Where("localizacao", "Comodato").Count();
            int rota = Item.Model.Where("localizacao", "Em Rota").Count();

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
