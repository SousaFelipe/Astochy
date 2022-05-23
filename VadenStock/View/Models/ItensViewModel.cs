using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class ItensViewModel
    {
        public static List<ItemType> TodosOsItens
        {
            get { return Item.Model.Select(); }
        }



        public static List<ItemType> ItensPorProduto(int produto)
        {
            return Item.Model
                .Where("produto", produto.ToString())
                .Select();
        }

        public static int CountItensPorProduto(int produto)
        {
            return Item.Model
                .Where("produto", produto.ToString())
                .Count();
        }

        public static List<ItemType> ItensPorProdutoByStatus(int produto, string status)
        {
            return Item.Model
                .Where("produto", produto.ToString())
                .Where("localizacao", status)
                .Select();
        }



        public static int CountItensPorAlmoxarifado(int almoxarifado)
        {
            return Item.Model
                .Where("almoxarifado", almoxarifado.ToString())
                .Count();
        }

        public static List<ItemType> ItensPorAlmoxarifado(int almoxarifado)
        {
            return Item.Model
                .Where("almoxarifado", almoxarifado.ToString())
                .Select();
        }
    }
}
