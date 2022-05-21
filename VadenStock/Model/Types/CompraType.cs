using System;



namespace VadenStock.Model.Types
{
    public struct CompraType : IModelType
    {
        public enum CompraStatus
        {
            Orcamento = 0,
            Aprovada,
            Recusada,
            Cancelada,
            Indefinido
        }



        public int Id { get; set; }
        public FornecedorType Fornecedor { get; set; }
        public string NumSerie { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataEmissao { get; set; }
        public CompraStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }



        public static string GetStatusName(CompraStatus status)
        {
            return status switch
            {
                CompraStatus.Orcamento => "Orcamento",
                CompraStatus.Aprovada => "Aprovada",
                CompraStatus.Recusada => "Recusada",
                CompraStatus.Cancelada => "Cancelada",
                _ => "Indefinido"
            };
        }



        public static CompraStatus GetStatus(string status)
        {
            return status switch
            {
                "Orcamento" => CompraStatus.Orcamento,
                "Aprovada" => CompraStatus.Aprovada,
                "Recusada" => CompraStatus.Recusada,
                "Cancelada" => CompraStatus.Cancelada,
                _ => CompraStatus.Indefinido,
            };
        }
    }
}
