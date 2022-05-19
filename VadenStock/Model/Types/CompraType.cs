using System;



namespace VadenStock.Model.Types
{
    public struct CompraType : IModelType
    {
        public int Id { get; set; }
        public FornecedorType Fornecedor { get; set; }
        public string NumSerie { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
