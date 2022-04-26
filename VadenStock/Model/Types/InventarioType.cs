using System;



namespace VadenStock.Model.Types
{
    public struct InventarioType
    {
        public int Id { get; set; }
        public CompraType Compra { get; set; }
        public double ValorTotal { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
