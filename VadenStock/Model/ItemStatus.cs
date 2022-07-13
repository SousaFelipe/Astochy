using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class ItemStatus : Connection
    {
        public static ItemStatus Model { get { return new ItemStatus(); } }



        public ItemStatus() : base("items_status") { }



        public override ItemStatus Where(string column, object operOrValue, object? value = null)
        {
            return (ItemStatus)base.Where(column, operOrValue, value);
        }



        public override ItemStatus Or(string column, object operOrValue, object? value = null)
        {
            return (ItemStatus)base.Or(column, operOrValue, value);
        }



        public List<ItemStatusType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<ItemStatusType> list = new();

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
            public static ItemStatusType Get(MySqlDataReader reader)
            {
                ItemStatusType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name")
                };

                return contract;
            }
        }
    }
}
