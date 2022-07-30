using System;
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



        public static int Create(ItemType item)
        {
            List<string[]> inserts = new()
            {
                new string[] { "codigo", item.Codigo },
                new string[] { "mac", item.Mac },
                new string[] { "produto", item.Produto.Id.ToString() },
                new string[] { "almoxarifado", item.Almoxarifado.Id.ToString() },
                new string[] { "compra", item.Compra.Id.ToString() },
                new string[] { "localizacao", ItemType.GetStatusName(item.Localizado) }
            };

            return Item.Model.Create(inserts);
        }



        public static List<ItemType> Read(params object[][] wheres)
        {
            Item model = Item.Model;

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
        }



        public static ItemType? Find(object codeOrMAC)
        {
            List<ItemType> loaded = Item.Model
                .Where("codigo", codeOrMAC)
                .Or("mac", codeOrMAC)
                .Select();

            return loaded.Count > 0
                ? loaded[0]
                : null;
        }



        public static int CountItensPorProduto(int produto)
        {
            return Item.Model
                .Where("produto", produto)
                .Where("orcamento", 0)
                .Count();
        }



        public static List<ItemType> ItensPorProdutoByStatus(int produto, string status)
        {
            return Item.Model
                .Where("produto", produto.ToString())
                .Where("localizacao", status)
                .Where("orcamento", "0")
                .Select();
        }



        public static int CountItensPorAlmoxarifado(int almoxarifado)
        {
            return Item.Model
                .Where("almoxarifado", almoxarifado.ToString())
                .Where("orcamento", "0")
                .Count();
        }



        public static List<ItemType> ItensPorAlmoxarifado(int almoxarifado, params object[][] wheres)
        {
            Item model = Item.Model
                .Where("orcamento", "0")
                .Where("almoxarifado", almoxarifado.ToString());

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
        }



        public static int Update(int id, string[] columns, object[] values)
        {
            return Item.Model
                .Where("id", id.ToString())
                .Update(columns, values);
        }
    }
}
