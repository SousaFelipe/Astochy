using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Tipo : Connection
    {
        public static Tipo New { get { return new Tipo(); } }



        public Tipo() : base("tipos") { }



        public List<TipoType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<TipoType> list = new();

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



        public override Tipo Count(string column = "*")
        {
            return (Tipo)base.Count(column);
        }



        public override Tipo Select(string[]? selects = null)
        {
            return (Tipo)base.Select(selects);
        }



        public override Tipo Where(string column, string oper, object? value = null)
        {
            return (Tipo)base.Where(column, oper, value);
        }



        private class Content
        {
            public static TipoType Get(MySqlDataReader reader)
            {
                TipoType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Categoria = Categoria.New.Get(reader.GetInt32("categoria"))[0],
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
