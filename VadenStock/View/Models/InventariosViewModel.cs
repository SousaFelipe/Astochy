using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class InventariosViewModel
    {
        public static InventarioType? Find(int id)
        {
            List<InventarioType> list = Inventario.Model
                .Where("id", id.ToString())
                .Select();

            return list.Count > 0
                ? list[0]
                : null;
        }
    }
}
