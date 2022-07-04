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



        public static AlmoxType? Find(int id)
        {
            List<AlmoxType> search = Almoxarifado.Model
                .Where("id", id.ToString())
                .Select();

            return search.Count > 0
                ? search[0]
                : null;
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
                            new string[] { "almoxarifado", "localizacao" },
                            new object[] { destino.Value.Id, ItemType.GetStatusName(destino.Value.Acao) }
                        );
                }
            }

            if (origem != null && destino != null)
            {
                List<string[]> inserts = new()
                {
                    new string[] { "itens", AlmoxTransfType.Implode(ids) },
                    new string[] { "from_almoxarifado", origem.Value.Id.ToString() },
                    new string[] { "to_almoxarifado", destino.Value.Id.ToString() },
                    new string[] { "acao", ItemType.GetStatusName(destino.Value.Acao) },
                    new string[] { "description", $"{Str.ZeroFill(num, " item".Pluralize(num, "n"))} {"transferido".Pluralize(num)} de '{origem.Value.Name}' para '{destino.Value.Name}'" }
                };

                return AlmoxarifadoTransferencia.Model.Create(inserts) > 0;
            }

            return false;
        }
    }
}
