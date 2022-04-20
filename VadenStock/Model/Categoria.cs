using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;



namespace VadenStock.Model
{
    public class Categoria : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
        }



        public static Categoria New { get { return new Categoria(); } }


        public Categoria() : base("categorias") { }



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
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
