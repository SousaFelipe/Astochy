using System;



namespace VadenStock.Model.Types
{
    public struct ItemType
    {
        public enum Status
        {
            Estoque,
            Rota,
            Producao,
            Comodato,
            Recolhido,
            Extraviado,
            Danificado,
            Vendido,
            Indefinido
        };



        public int Id { get; set; }
        public ProdutoType Produto { get; set; }
        public AlmoxType Almoxarifado { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Localizado { get; set; }



        public static Status GetStatus(string status)
        {
            return status switch
            {
                "Estoque" => Status.Estoque,
                "Producao" => Status.Producao,
                "Comodato" => Status.Comodato,
                "Extraviado" => Status.Extraviado,
                "Danificado" => Status.Danificado,
                "Vendido" => Status.Vendido,
                _ => Status.Indefinido
            };
        }
    }
}
