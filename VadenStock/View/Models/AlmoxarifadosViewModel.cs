using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View.Models
{
    public static class AlmoxarifadosViewModel
    {
        public static List<AlmoxType> TodosOsAlmoxarifados
        {
            get { return Almoxarifado.Model.Select(); }
        }



        public static AlmoxType? Find(params object[][] wheres)
        {
            Almoxarifado model = Almoxarifado.Model;

            foreach (object[] w in wheres)
                model.Where(w[0].ToString(), w[1]);

            List<AlmoxType> search = model.Select();

            return search.Count > 0
                ? search[0]
                : null;
        }


        
        public static int Update(int id, params KeyValuePair<string, object>[] pairs)
        {
            string[] columns = new string[pairs.Length];
            object[] values = new string[pairs.Length];

            int pair;
            for (pair = 0; pair < pairs.Length; pair++)
            {
                columns[pair] = pairs[pair].Key;
                values[pair] = pairs[pair].Value;
            }

            return Almoxarifado.Model
                .Where("id", id)
                .Update(columns, values);
        }



        public static bool Transferir(AlmoxType? origem, AlmoxType? destino, List<ItemType?> itens)
        {
            ItemType? item;
            int num = itens.Count;
            int[] ids = new int[itens.Count];

            for (int i = 0; i < itens.Count; i++)
            {
                item = itens[i];

                if (item != null && destino != null)
                {
                    ids[i] = item.Value.Id;

                    _ = ItensViewModel.Update(
                            ids[i],
                            new string[] {
                                "almoxarifado",
                                "localizacao",
                                "ultima_transf"
                            },
                            new object[] {
                                destino.Value.Id,
                                ItemType.GetStatusName(destino.Value.Acao),
                                DateTime.Now
                            }
                        );
                }
            }

            if (origem != null && destino != null)
            {
                List<object[]> inserts = new()
                {
                    new object[] { "itens", TransfType.Implode(ids) },
                    new object[] { "from_almoxarifado", origem.Value.Id },
                    new object[] { "to_almoxarifado", destino.Value.Id },
                    new object[] { "acao", ItemType.GetStatusName(destino.Value.Acao) },
                    new object[] { "description", $"{Str.ZeroFill(num, " item".Pluralize(num, "n"))} {"transferido".Pluralize(num)} de '{origem.Value.Name}' para '{destino.Value.Name}'" }
                };

                return AlmoxarofadoTransferencia.Model.Create(inserts) > 0;
            }

            return false;
        }
    }
}
