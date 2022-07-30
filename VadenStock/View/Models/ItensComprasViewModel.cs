using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class ItensComprasViewModel
    {
        public static bool Create(ItemCompraType item)
        {
            if (item.Compra.Id <= 0)
                return false;

            List<string[]> inserts = new()
            {
                new string[] { "compra", item.Compra.Id.ToString() },
                new string[] { "produto", item.Produto.Id.ToString() },
                new string[] { "quantidade", item.Quantidade.ToString() },
                new string[] { "valor_total", item.ValorTotal.ToString().Replace(".", "").Replace(",", ".") }
            };

            return ItemCompra.Model.Create(inserts) > 0;
        }
    }
}
