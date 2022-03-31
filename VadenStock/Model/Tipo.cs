﻿using System;

using MySql.Data.MySqlClient;



namespace VadenStock.Model
{
    public class Tipo : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public Categoria.Contract Categoria { get; set; }
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
        }



        public Contract? Load(int id)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    using (Cmmd = new MySqlCommand(RawLoad("tipos", id), Plug))
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
            #pragma warning disable CS8629
            public static Contract Get(MySqlDataReader reader)
            {
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Categoria = (Categoria.Contract)new Categoria().Load(reader.GetInt32("categoria")),
                    Name = reader.GetString("name"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}