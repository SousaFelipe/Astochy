using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Config : Connection
    {
        public static Config Model { get { return new Config(); } }



        public Config() : base("configs") { }



        public override Config Where(string column, object operOrValue, object? value = null)
        {
            return (Config)base.Where(column, operOrValue, value);
        }



        public List<ConfigType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<ConfigType> list = new();

                    using (Cmmd = new MySqlCommand(Builder.Query, Plug))
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
            public static ConfigType Get(MySqlDataReader reader)
            {
                ConfigType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    ProductionPath = reader.IsDBNull(1) ? string.Empty : reader.GetString("production_path"),
                    AlmoxPrincipal = reader.IsDBNull(2) ? null : Almoxarifado.Model.Where("id", reader.GetInt32("almox_principal")).Select()[0],
                    ServerAddress = reader.GetString("server_address"),
                    ServerProtocol = reader.IsDBNull(4) ? ConfigType.Protocol.HTTP : ConfigType.GetProtocol(reader.GetString("server_protocol")),
                    ServerToken = reader.GetString("server_token"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
