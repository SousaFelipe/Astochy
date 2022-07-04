using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Almoxarifado : Connection
    {
        public static Almoxarifado Model { get { return new Almoxarifado(); } }



        public Almoxarifado () : base("almoxarifados") { }



        public override Almoxarifado Where(string column, object operOrValue, object? value = null)
        {
            return (Almoxarifado)base.Where(column, operOrValue, value);
        }



        public override Almoxarifado Or(string column, object operOrValue, object? value = null)
        {
            return (Almoxarifado)base.Or(column, operOrValue, value);
        }



        public List<AlmoxType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<AlmoxType> list = new();

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
            public static AlmoxType Get(MySqlDataReader reader)
            {
                AlmoxType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Tipo = AlmoxType.GetTipo(reader.GetString("tipo")),
                    Acao = ItemType.GetStatus(reader.GetString("acao")),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(4) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
