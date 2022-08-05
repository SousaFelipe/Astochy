using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Inventario : Connection
    {
        public static Inventario Model { get { return new Inventario(); } }



        public Inventario() : base("inventarios") { }



        public override Inventario Where(string column, object operOrValue, object? value = null)
        {
            return (Inventario)base.Where(column, operOrValue, value);
        }



        public List<InventarioType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<InventarioType> list = new();

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
            public static InventarioType Get(MySqlDataReader reader)
            {
                InventarioType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Compra = Compra.Model.Where("id", reader.GetInt32("compra")).Select()[0],
                    ValorTotal = reader.GetDouble("valor_total") * 100,
                    CreatedDate = reader.IsDBNull(3) ? System.DateTime.MinValue : reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
