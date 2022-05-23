using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Produto : Connection
    {
        public static Produto Model { get { return new Produto(); } }



        public Produto() : base("produtos") { }



        public override Produto Where(string column, object operOrValue, object? value = null)
        {
            return (Produto)base.Where(column, operOrValue, value);
        }



        public List<ProdutoType> Select(params string[]? selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<ProdutoType> list = new();

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
            public static ProdutoType Get(MySqlDataReader reader)
            {
                ProdutoType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Categoria = Categoria.Model.Where("id", reader.GetInt32("categoria")).Select()[0],
                    Tipo = Tipo.Model.Where("id", reader.GetInt32("tipo")).Select()[0],
                    Marca = Marca.Model.Where("id", reader.GetInt32("marca")).Select()[0],
                    Name = reader.GetString("name"),
                    Image = reader.IsDBNull(5) ? string.Empty : reader.GetString("image"),
                    Description = reader.IsDBNull(6) ? string.Empty : reader.GetString("description"),
                    Price = reader.GetDecimal("price"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
