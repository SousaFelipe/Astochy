using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;
using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class DashboardViewModel
    {
        public static List<CategoriaType> GetCategorias()
        {
            return Categoria.New.Select().Get();
        }



        public static List<TipoType> GetTipos(int categoria = 0)
        {
            return Tipo.New
                .Select()
                .Where("categoria", categoria.ToString())
                .Get();
        }



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



        public static List<AlmoxType> GetAlmoxarifados()
        {
            return Almoxarifado.New
                .Select()
                .Get();
        }



        public static List<ItemType> GetItems(int almoxarifado)
        {
            return Item.New
                .Select()
                .Where("almoxarifado", almoxarifado.ToString())
                .Get();
        }
    }
}
