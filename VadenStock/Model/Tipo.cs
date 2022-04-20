using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;



namespace VadenStock.Model
{
    public class Tipo : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public Categoria.Contract Categoria { get; set; }
            public string Name { get; set; }
            public string Description { get; set;  }
            public DateTime CreatedDate { get; set; }
        }



        public static Tipo New { get { return new Tipo(); } }



        public Tipo() : base("tipos") { }



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
            #pragma warning disable CS8629
            public static Contract Get(MySqlDataReader reader)
            {
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Categoria = (Categoria.Contract)new Categoria().Get(reader.GetInt32("categoria"))[0],
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
