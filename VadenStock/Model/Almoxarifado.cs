using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Almoxarifado : Connection
    {
        public static Almoxarifado New { get { return new Almoxarifado(); } }



        public Almoxarifado () : base("almoxarifados") { }



        public List<AlmoxType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<AlmoxType> list = new();

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



        public override Almoxarifado Count(string column = "*")
        {
            return (Almoxarifado)base.Count(column);
        }



        public override Almoxarifado Select(string[]? selects = null)
        {
            return (Almoxarifado)base.Select(selects);
        }



        public override Almoxarifado Where(string column, string operOrValue, object? value = null)
        {
            return (Almoxarifado)base.Where(column, operOrValue, value);
        }



        private class Content
        {
            public static AlmoxType Get(MySqlDataReader reader)
            {
                AlmoxType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Tipo = AlmoxType.GetTipo(reader.GetString("tipo")),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
