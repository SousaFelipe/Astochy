using System;



namespace VadenStock.Model.Types
{
    public struct ItemCompraType
    {
        public enum ICStatus
        {
            Aberto,
            Baixado
        }



        public int Id { get; set; }
        public CompraType Compra { get; set; }
        public ProdutoType Produto { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public ICStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }



        public static ICStatus GetStatus(string status)
        {
            return status switch
            {
                "Aberto" => ICStatus.Aberto,
                _ => ICStatus.Baixado,
            };
        }



        public static string GetStatusName(ICStatus status)
        {
            return status switch
            {
                ICStatus.Aberto => "Aberto",
                _ => "Baixado",
            };
        }
    }
}
