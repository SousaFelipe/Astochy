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



        public static ItemType? Find(object search)
        {
            List<ItemType> loaded = Item.Model
                .Where("codigo", search)
                .Or("mac", search)
                .Select();

            return loaded.Count > 0
                ? loaded[0]
                : null;
        }



        public static int Create(ItemType item)
        {
            List<string[]> inserts = new()
            {
                new string[] { "codigo", item.Codigo },
                new string[] { "mac", item.Mac },
                new string[] { "produto", item.Produto.Id.ToString() },
                new string[] { "almoxarifado", item.Almoxarifado.Id.ToString() },
                new string[] { "inventario", item.Inventario.Id.ToString() },
                new string[] { "localizacao", ItemType.GetStatusName(item.Localizado) }
            };

            return Item.Model.Create(inserts);
        }



        public static List<ItemType> ItensPorProduto(int produto, params string[][] query)
        {
            Item model = Item.Model
                .Where("produto", produto.ToString());

            if (query.Length > 0)
                foreach (string[] q in query)
                    model.Where(Convert.ToString(q[0]), Convert.ToString(q[1]));

            return model.Select();
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

        public static List<ItemType> ItensPorAlmoxarifado(int almoxarifado, params object[][] query)
        {
            Item model = Item.Model
                .Where("almoxarifado", almoxarifado.ToString());

            if (query.Length > 0)
                foreach (object[] q in query)
                    model.Where(Convert.ToString(q[0]), Convert.ToString(q[1]));

            return model.Select();
        }



        public static int CountItensPorCategoria(int categoria)
        {
            int count = 0;

            foreach(ProdutoType p in ProdutosViewModel.ProdutosPorCategoria(categoria))
                count += Item.Model.Where("produto", p.Id.ToString()).Count();

            return count;
        }



        public static int Update(int id, string[] columns, object[] values)
        {
            return Item.Model
                .Where("id", id.ToString())
                .Update(columns, values);
        }
    }
}
