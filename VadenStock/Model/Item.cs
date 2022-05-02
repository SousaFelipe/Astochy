using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Item : Connection
    {
        public static Item New {  get { return new Item(); } }



        public Item() : base ("items") { }



        public List<ItemType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<ItemType> list = new();

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



        public override Item Count(string column = "*")
        {
            return (Item)base.Count(column);
        }



        public override Item Select(string[]? selects = null)
        {
            return (Item)base.Select(selects);
        }



        public override Item Where(string column, string operOrValue, object? value = null)
        {
            return (Item)base.Where(column, operOrValue, value);
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
                    Produto = Produto.New.Get(reader.GetInt32("produto"))[0],
                    Almoxarifado = Almoxarifado.New.Get(reader.GetInt32("almoxarifado"))[0],
                    Inventario = Inventario.New.Get(reader.GetInt32("inventario"))[0],
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(4) ? string.Empty : reader.GetString("description"),
                    Localizado = ItemType.GetStatus(reader.GetString("localizacao")),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
