using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Inventario : Connection
    {
        public static Inventario New { get { return new Inventario(); } }



        public Inventario() : base("inventarios") { }



        public List<InventarioType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<InventarioType> list = new();

                    string? query = (id > 0)
                        ? Builder.Load(id)
                        : Builder.SQL();

                    using (Cmmd = new MySqlCommand(query, Plug))
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



        public override Inventario Count(string column = "*")
        {
            return (Inventario)base.Count(column);
        }



        public override Inventario Select(string[]? selects = null)
        {
            return (Inventario)base.Select(selects);
        }



        public override Inventario Where(string column, string operOrValue, object? value = null)
        {
            return (Inventario)base.Where(column, operOrValue, value);
        }



        public override Inventario InnerJoin(string table1, string column1, string? table2 = null, string column2 = "id")
        {
            return (Inventario)base.InnerJoin(table1, column1, table2, column2);
        }



        private class Content
        {
            public static InventarioType Get(MySqlDataReader reader)
            {
                InventarioType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Compra = Compra.New.Get(reader.GetInt32("compra"))[0],
                    ValorTotal = reader.GetDouble("valor_total"),
                    CreatedDate = reader.IsDBNull(3) ? System.DateTime.MinValue : reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
