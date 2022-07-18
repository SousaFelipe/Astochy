using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class ConfigsViewModel
    {
        public static bool Configured()
        {
            List<ConfigType> configs = Config.Model.Where("id", 1).Select();
            return configs != null && configs.Count >= 1;
        }



        public static bool Create(ConfigType config)
        {
            List<string[]> inserts = new()
            {
                new string[] { "production_path", config.ProductionPath },
                new string[] { "server_address", config.ServerAddress },
                new string[] { "server_protocol", config.ServerProtocol.ToString() },
                new string[] { "server_token", config.ServerToken },
            };

            return Config.Model.Create(inserts) > 0;
        }
    }
}
