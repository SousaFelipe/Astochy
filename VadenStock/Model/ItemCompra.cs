using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class ItemCompra : Connection
    {
        public static ItemCompra Model { get { return new ItemCompra(); } }



        public ItemCompra() : base("itens_compra") { }



        public override ItemCompra Where(string column, object operOrValue, object? value = null)
        {
            return (ItemCompra)base.Where(column, operOrValue, value);
        }



        public override ItemCompra Or(string column, object operOrValue, object? value = null)
        {
            return (ItemCompra)base.Or(column, operOrValue, value);
        }



        public List<ItemCompraType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<ItemCompraType> list = new();

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
            public static ItemCompraType Get(MySqlDataReader reader)
            {
                ItemCompraType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Compra = Compra.Model.Where("id", reader.GetInt32("compra")).Select()[0],
                    Produto = Produto.Model.Where("id", reader.GetInt32("produto")).Select()[0],
                    Quantidade = reader.GetInt32("quantidade"),
                    ValorTotal = reader.GetDouble("valor_total"),
                    Status = ItemCompraType.GetStatus(reader.GetString("status")),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
