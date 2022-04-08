using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;



namespace VadenStock.Model
{
    public class Produto : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public Tipo.Contract Tipo { get; set; }
            public Marca.Contract Marca { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public DateTime CreatedDate { get; set; }
        }



        public Produto() : base("produtos") { }



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
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Tipo = new Tipo().Get(reader.GetInt32("tipo"))[0],
                    Marca = new Marca().Get(reader.GetInt32("marca"))[0],
                    Code = reader.GetString("code"),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(6) ? string.Empty : reader.GetString("description"),
                    Price = reader.GetDecimal("price"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
