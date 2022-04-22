using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.View.Structs;



namespace VadenStock.View.Models
{
    public class DashboardViewModel
    {
        public static List<Categoria.Contract> Categorias
        {
            get { return ((Categoria)Categoria.New.Select()).Get(); }
        }



        public static List<Tipo.Contract> GetTipos(int categoria = 0)
        {
            return (
                
                    (Tipo)Tipo.New
                        .Select()
                        .Where("categoria", categoria.ToString())
                
                ).Get();
        }



        public static PatrimonioS GetPatrimonio()
        {
            int e = Item.New.Count().Where("localizacao", "Estoque").Bind();
            int c = Item.New.Count().Where("localizacao", "Comodato").Bind();
            int p = Item.New.Count().Where("localizacao", "Producao").Bind();

            PatrimonioS patrimonio = new()
            {
                Estoque = e,
                Comodato = c,
                Producao = p,
                Total = e + c + p
            };

            return patrimonio;
        }



        public static List<Almoxarifado.Contract> GetAlmoxarifados()
        {
            return ((Almoxarifado)Almoxarifado.New.Select()).Get();
        }



        public static List<Item.Contract> GetItems(int almoxarifado)
        {
            return (

                    (Item)Item.New
                        .Select()
                        .Where("almoxarifado", almoxarifado.ToString())

                ).Get();
        }
    }
}
