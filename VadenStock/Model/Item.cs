using System;
using System.Collections.Generic;

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



        public static Item New {  get { return new Item(); } }



        public Item() : base ("items") { }



        public List<Contract> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<Contract> list = new();

                    string? query = id > 0
                        ? Builder.Load(id)
                        : Builder.Query;

                    using (Cmmd = new MySqlCommand(query, Plug))
                    {
                        using (Reader = Cmmd.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                list.Add(Content.Get(Reader));
                            }

                            return list;
                        }
                    }
                }
            }
            finally
            {
                Unplug();
            }
        }



        private class Content
        {
            public static Contract Get(MySqlDataReader reader)
            {
                #pragma warning disable CS8629
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Produto = new Produto().Get(reader.GetInt32("produto"))[0],
                    Almoxarifado = new Almoxarifado().Get(reader.GetInt32("almoxarifado"))[0],
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
