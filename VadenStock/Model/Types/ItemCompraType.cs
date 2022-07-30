using System;



namespace VadenStock.Model.Types
{
    public struct ItemCompraType
    {
        public int Id { get; set; }
        public CompraType Compra { get; set; }
        public ProdutoType Produto { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
