using System;



namespace VadenStock.Model.Types
{
    public struct ItemType
    {
        public enum Status
        {
            Desconhecido = 0,

            Comodato,
            Conserto,
            Danificado,
            Estoque,
            Extraviado,
            Producao,
            Recolhido,
            Rota,
            Vendido
        };



        public static readonly string[] STATUS = {
            "Comodato",
            "Conserto",
            "Danificado",
            "Desconhecido",
            "Estoque",
            "Rota",
            "Extraviado",
            "Producao",
            "Recolhido",
            "Vendido"
        };



        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Mac { get; set; }
        public ProdutoType Produto { get; set; }
        public AlmoxType Almoxarifado { get; set; }
        public InventarioType Inventario { get; set; }
        public string Description { get; set; }
        public Status Localizado { get; set; }
        public DateTime UltimaTransf { get; set; }
        public DateTime CreatedDate { get; set; }



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
                "Rota" => Status.Rota,
                "Vendido" => Status.Vendido,

                _ => Status.Desconhecido
            };
        }



        public static string GetStatusName(Status status)
        {
            return status switch
            {
                Status.Comodato => "Comodato",
                Status.Conserto => "Conserto",
                Status.Danificado => "Danificado",
                Status.Desconhecido => "Desconhecido",
                Status.Estoque => "Estoque",
                Status.Extraviado => "Extraviado",
                Status.Producao => "Producao",
                Status.Recolhido => "Recolhido",
                Status.Rota => "Rota",
                Status.Vendido => "Vendido",

                _ => "Indefinido"
            };
        }
    }
}
