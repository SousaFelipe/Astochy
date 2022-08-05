using System;



namespace VadenStock.Model.Types
{
    public struct AlmoxType
    {
        public enum Hosted
        {
            Estoque,
            Carro,
            Moto,
            Indefinido
        }



        public int Id { get; set; }
        public Hosted Tipo { get; set; }
        public ItemType.Status Acao { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Listagem { get; set; }
        public DateTime CreatedDate { get; set; }



        public static readonly string[] ACOES = new string[]
        {
            "Comodato",
            "Conserto",
            "Danificado",
            "Desconhecido",
            "Estoque",
            "Extraviado",
            "Producao",
            "Recolhido",
            "Rota",
            "Vendido"
        };



        public string GetIcon(string color = "blue")
        {
            string icon = Tipo == Hosted.Carro
                ? "car"
                : Tipo == Hosted.Moto
                    ? "bike"
                    : "warehouse";

            return $"{color}-{icon}";
        }



        public static Hosted GetTipo(string tipo)
        {
            return tipo switch
            {
                "E" => Hosted.Estoque,
                "C" => Hosted.Carro,
                "M" => Hosted.Moto,
                _ => Hosted.Indefinido
            };
        }



        public static string GetTipoName(Hosted tipo)
        {
            return tipo switch
            {
                Hosted.Estoque => "E",
                Hosted.Carro => "C",
                Hosted.Moto => "M",
                _ => "0"
            };
        }
    }
}
