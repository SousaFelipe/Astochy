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



        public static bool Create(ItemType item)
        {
            List<object[]> inserts = new()
            {
                new object[] { "codigo", item.Codigo },
                new object[] { "mac", item.Mac },
                new object[] { "produto", item.Produto.Id },
                new object[] { "almoxarifado", item.Almoxarifado.Id },
                new object[] { "localizacao", ItemType.GetStatusName(item.Localizado) }
            };

            if (item.Compra != null)
                inserts.Add(new object[] { "compra", item.Compra.Id });

            if (item.UltimaTransf != null)
                inserts.Add(new object[] { "ultima_transf", item.UltimaTransf });

            inserts.Add(new object[] { "valor", item.Valor });

            return Item.Model.Create(inserts) > 0;
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
                .Where("almoxarifado", almoxarifado)
                .Count();
        }



        public static List<ItemType> ItensPorAlmoxarifado(int almoxarifado, params object[][] wheres)
        {
            Item model = Item.Model
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
