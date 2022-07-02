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



        public static int CountItensPorCategoria(int categoria)
        {
            int count = 0;

            foreach(ProdutoType p in ProdutosViewModel.ProdutosPorCategoria(categoria))
                count += Item.Model.Where("produto", p.Id.ToString()).Count();

            return count;
        }
    }
}
