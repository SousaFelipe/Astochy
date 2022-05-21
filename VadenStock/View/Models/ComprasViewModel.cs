using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class ComprasViewModel
    {
        public static List<CompraType> ComprasPorStatus(CompraType.CompraStatus status)
        {
            return Compra.Model.Select()
                .Where("status", CompraType.GetStatusName(status))
                .Get();
        }
    }
}
