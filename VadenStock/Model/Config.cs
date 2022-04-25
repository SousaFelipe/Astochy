using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Config : Connection
    {
        public static Config New { get { return new Config(); } }



        public Config() : base("configs") { }



        public List<ConfigType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<ConfigType> list = new();

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



        public override Config Count(string column = "*")
        {
            return (Config)base.Count(column);
        }



        public override Config Select(string[]? selects = null)
        {
            return (Config)base.Select(selects);
        }



        public override Config Where(string column, string operOrValue, object? value = null)
        {
            return (Config)base.Where(column, operOrValue, value);
        }



        private class Content
        {
            public static ConfigType Get(MySqlDataReader reader)
            {
                ConfigType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    ProductionPath = reader.GetString("production_path"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
