using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public class ConfigsViewModel
    {
        public static ConfigType? Default
        {
            get
            {
                if (Configured())
                    return Config.Model.Where("id", 1).Select()[0];

                return null;
            }
        }



        public static bool Configured()
        {
            List<ConfigType> configs = Config.Model.Where("id", 1).Select();
            return configs != null && configs.Count >= 1;
        }



        public static bool Create(ConfigType config)
        {
            List<object[]> inserts = new()
            {
                new object[] { "production_path", config.ProductionPath },
                new object[] { "server_address", config.ServerAddress },
                new object[] { "server_protocol", config.ServerProtocol.ToString() },
                new object[] { "server_token", config.ServerToken },
            };

            return Config.Model.Create(inserts) > 0;
        }
    }
}
