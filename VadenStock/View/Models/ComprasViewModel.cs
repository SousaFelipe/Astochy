using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class ComprasViewModel
    {
        public static bool Create(CompraType compra)
        {
            List<object[]> inserts = new()
            {
                new object[] { "fornecedor", compra.Fornecedor.Id },
                new object[] { "valor_total", compra.ValorTotal },
                new object[] { "status", CompraType.GetStatusName(compra.Status) }
            };

            return Compra.Model.Create(inserts) > 0;
        }



        public static List<CompraType> Read(params string[][] wheres)
        {
            Compra model = Compra.Model;

            foreach (string[] where in wheres)
                model.Where(Convert.ToString(where[0]), where[1]);

            List<CompraType> compras = model.Select();

            return compras ?? new List<CompraType>();
        }



        public static CompraType? Last()
        {
            List<CompraType> compras = Compra.Model
                .Where("id", ">", 1)
                .OrderBy("id", "DESC");

            return (compras != null && compras.Count > 0)
                ? compras[0]
                : null;
        }



        public static List<CompraType> ComprasPorStatus(CompraType.CompraStatus status)
        {
            return Compra.Model
                .Where("id", ">", 1)
                .Where("status", CompraType.GetStatusName(status))
                .Select();
        }



        public static int Update(CompraType compra)
        {
            string[] columns = new string[]
            {
                "fornecedor",
                "valor_total",
                "status",
                "updated_at"
            };

            object[] values = new object[]
            {
                compra.Fornecedor.Id,
                compra.ValorTotal,
                CompraType.GetStatusName(compra.Status),
                DateTime.Now
            };

            return Compra.Model
                .Where("id", compra.Id)
                .Update(columns, values);
        }



        public static bool Delete(int id)
        {
            return Compra.Model.Delete(id);
        }
    }
}
