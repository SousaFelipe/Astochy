using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Tipo : Connection
    {
        public static Tipo Model { get { return new Tipo(); } }



        public Tipo() : base("tipos") { }



        public override Tipo Where(string column, object operOrValue, object? value = null)
        {
            return (Tipo)base.Where(column, operOrValue, value);
        }



        public List<TipoType> Select(params string[]? selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<TipoType> list = new();

                    using (Cmmd = new MySqlCommand(Builder.Query, Plug))
                    {
                        using (Reader = Cmmd.ExecuteReader())
                        {
                            while (Reader.Read())
                                list.Add(Content.Get(Reader));

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
            public static TipoType Get(MySqlDataReader reader)
            {
                TipoType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Categoria = Categoria.Model.Where("id", reader.GetInt32("categoria")).Select()[0],
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
