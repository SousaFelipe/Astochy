using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class ItemStatusViewModel
    {
        public static List<ItemStatusType> TodosOsItemStatus
        {
            get { return ItemStatus.Model.Select(); }
        }
    }
}
