using System;



namespace VadenStock.Model.Types
{
    public struct ItemType
    {
        public enum Status
        {
            Indefinido = 0,

            Comodato,
            Conserto,
            Danificado,
            Estoque,
            Extraviado,
            Producao,
            Recolhido,
            EmRota,
            Vendido
        };



        public static readonly string[] STATUS = {
            "Desconhecido", 

            "Comodato",
            "Conserto",
            "Danificado",
            "Estoque",
            "Extraviado",
            "Producao",
            "Recolhido",
            "EmRota",
            "Vendido"
        };



        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Mac { get; set; }
        public ProdutoType Produto { get; set; }
        public AlmoxType Almoxarifado { get; set; }
        public InventarioType Inventario { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Localizado { get; set; }



        public static Status GetStatus(string status)
        {
            return status switch
            {
                "Comodato" => Status.Comodato,
                "Conserto" => Status.Conserto,
                "Danificado" => Status.Danificado,
                "Estoque" => Status.Estoque,
                "Extraviado" => Status.Extraviado,
                "Producao" => Status.Producao,
                "Recolhido" => Status.Recolhido,
                "Em Rota" => Status.EmRota,
                "Vendido" => Status.Vendido,

                _ => Status.Indefinido
            };
        }



        public static string GetStatusName(Status status)
        {
            return status switch
            {
                Status.Comodato => "Comodato",
                Status.Conserto => "Conserto",
                Status.Danificado => "Danificado",
                Status.Estoque => "Estoque",
                Status.Extraviado => "Extraviado",
                Status.Producao => "Producao",
                Status.Recolhido => "Recolhido",
                Status.EmRota => "EmRota",
                Status.Vendido => "Vendido",

                _ => "Desconhecido"
            };
        }
    }
}
