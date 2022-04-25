using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Marca : Connection
    {
        public static Marca New { get { return new Marca(); } }



        public Marca() : base("marcas") { }



        public List<MarcaType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<MarcaType> list = new();

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



        public override Marca Count(string column = "*")
        {
            return (Marca)base.Count(column);
        }


        public override Marca Select(string[]? selects = null)
        {
            return (Marca)base.Select(selects);
        }



        public override Marca Where(string column, string oper, object? value = null)
        {
            return (Marca)base.Where(column, oper, value);
        }



        private class Content
        {
            public static MarcaType Get(MySqlDataReader reader)
            {
                MarcaType contract = new()
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
