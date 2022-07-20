using System;
using System.Collections.Generic;



namespace VadenStock.Model.Types
{
    public class TransfType : IModelType
    {
        public int Id { get; set; }
        public string? Itens { get; set; }
        public AlmoxType From { get; set; }
        public AlmoxType To { get; set; }
        public ItemType.Status Acao { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }



        public bool ContainsItem(int id)
        {
            foreach(int itemID in ExplodeItens())
            {
                if (id == itemID)
                    return true;
            }

            return false;
        }



        public int[] ExplodeItens()
        {
            if (Itens != null)
            {
                string[] exploded = Itens.Split(";");
                int[] idList = new int[exploded.Length];

                for (int i = 0; i < idList.Length; i++)
                    idList[i] = Convert.ToInt32(exploded[i]);

                return idList;
            }

            return Array.Empty<int>();
        }



        public static string Implode(int[] itens)
        {
            string imploeded = $";{itens[0]};";

            if (itens.Length > 1)
                for (int i = 1; i < itens.Length; i++)
                    imploeded = string.Concat(imploeded, $"{itens[i]};");

            return imploeded;
        }
    }
}
