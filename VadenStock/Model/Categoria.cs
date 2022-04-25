using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Categoria : Connection
    {
        public static Categoria New { get { return new Categoria(); } }


        public Categoria() : base("categorias") { }



        public List<CategoriaType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<CategoriaType> list = new();

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



        public override Categoria Count(string column = "*")
        {
            return (Categoria)base.Count(column);
        }



        public override Categoria Select(string[]? selects = null)
        {
            return (Categoria)base.Select(selects);
        }



        public override Categoria Where(string column, string operOrValue, object? value = null)
        {
            return (Categoria)base.Where(column, operOrValue, value);
        }



        private class Content
        {
            public static CategoriaType Get(MySqlDataReader reader)
            {
                CategoriaType contract = new()
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
