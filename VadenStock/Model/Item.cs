using System;

using MySql.Data.MySqlClient;



namespace VadenStock.Model.Cadastros
{
    public class Item : Connection
    {
        public struct Contract
        {
            public enum Status
            {
                Estoque,
                Producao,
                Comodato,
                Extraviado,
                Danificado,
                Vendido,
                Indefinido
            };


            public int Id { get; set; }
            public Produto.Contract Produto { get; set; }
            public Almoxarifado.Contract Almoxarifado { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public Status Localizaado { get; set; }



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



        public Item() : base ("items") { }



        public Contract? Load(int id)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    using (Cmmd = new MySqlCommand(Builder.Load(id), Plug))
                    {
                        using (Reader = Cmmd.ExecuteReader())
                        {
                            if (Reader.Read())
                                return Content.Get(Reader);
                        }
                    }
                }
            }
            finally
            {
                Unplug();
            }

            return null;
        }



        private class Content
        {
            public static Contract Get(MySqlDataReader reader)
            {
                #pragma warning disable CS8629
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Produto = (Produto.Contract)new Produto().Load(reader.GetInt32("produto")),
                    Almoxarifado = (Almoxarifado.Contract)new Almoxarifado().Load(reader.GetInt32("almoxarifado")),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(4) ? string.Empty : reader.GetString("description"),
                    Localizaado = Contract.GetStatus(reader.GetString("localizado")),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
