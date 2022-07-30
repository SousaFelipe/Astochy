using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Item : Connection
    {
        public static Item Model {  get { return new Item(); } }



        public Item() : base ("itens") { }



        public override Item Where(string column, object operOrValue, object? value = null)
        {
            return (Item)base.Where(column, operOrValue, value);
        }



        public override Item Or(string column, object operOrValue, object? value = null)
        {
            return (Item)base.Or(column, operOrValue, value);
        }



        public List<ItemType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<ItemType> list = new();

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
            public static ItemType Get(MySqlDataReader reader)
            {
                ItemType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Codigo = reader.GetString("codigo"),
                    Mac = reader.IsDBNull(2) ? string.Empty : reader.GetString("mac"),
                    Produto = Produto.Model.Where("id", reader.GetInt32("produto")).Select()[0],
                    Almoxarifado = Almoxarifado.Model.Where("id", reader.GetInt32("almoxarifado")).Select()[0],
                    Compra = Compra.Model.Where("id", reader.GetInt32("compra")).Select()[0],
                    Description = reader.IsDBNull(6) ? string.Empty : reader.GetString("description"),
                    Localizado = ItemType.GetStatus(reader.GetString("localizacao")),
                    UltimaTransf = reader.IsDBNull(8) ? reader.GetDateTime("created_at") : reader.GetDateTime("ultima_transf"),
                    Orcamento = (reader.GetInt32("orcamento") > 0),
                    Valor = reader.GetDouble("valor"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
