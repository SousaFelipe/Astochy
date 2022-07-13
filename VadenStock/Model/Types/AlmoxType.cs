using System;



namespace VadenStock.Model.Types
{
    public struct AlmoxType : IModelType
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
        public DateTime CreatedDate { get; set; }



        public static string GetTipo(Hosted tipo)
        {
            return tipo switch
            {
                Hosted.Estoque => "E",
                Hosted.Carro => "C",
                Hosted.Moto => "M",
                _ => "0"
            };
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
    }
}
