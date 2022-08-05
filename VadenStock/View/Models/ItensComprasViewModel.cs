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

            List<object[]> inserts = new()
            {
                new object[] { "compra", item.Compra.Id },
                new object[] { "produto", item.Produto.Id },
                new object[] { "quantidade", item.Quantidade },
                new object[] { "valor_total", item.ValorTotal }
            };

            return ItemCompra.Model.Create(inserts) > 0;
        }



        public static List<ItemCompraType> Read(params object[][] wheres)
        {
            ItemCompra model = ItemCompra.Model;

            foreach (object[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            return model.Select();
        }



        public static int Update(ItemCompraType item)
        {
            string[] columns = new string[]
            {
                "compra",
                "produto",
                "quantidade",
                "status",
                "valor_total"
            };

            object[] values = new object[]
            {
                item.Compra.Id,
                item.Produto.Id,
                item.Quantidade,
                ItemCompraType.GetStatusName(item.Status),
                item.ValorTotal
            };

            return ItemCompra.Model
                .Where("id", item.Id)
                .Update(columns, values);
        }



        public static bool Delete(int id)
        {
            return ItemCompra.Model.Delete(id);
        }
    }
}
