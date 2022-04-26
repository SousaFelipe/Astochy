using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Produto : Connection
    {
        public static Produto New { get { return new Produto(); } }



        public Produto() : base("produtos") { }



        public List<ProdutoType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<ProdutoType> list = new();

                    string? query = (id > 0)
                        ? Builder.Load(id)
                        : Builder.SQL();

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



        public override Produto Count(string column = "*")
        {
            return (Produto)base.Count(column);
        }



        public override Produto Select(string[]? selects = null)
        {
            return (Produto)base.Select(selects);
        }



        public override Produto Where(string column, string operOrValue, object? value = null)
        {
            return (Produto)base.Where(column, operOrValue, value);
        }



        public override Produto InnerJoin(string table, string joinOne, string joinTwo = "id")
        {
            return (Produto)base.InnerJoin(table, joinOne, joinTwo);
        }



        private class Content
        {
            public static ProdutoType Get(MySqlDataReader reader)
            {
                ProdutoType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Tipo = Tipo.New.Get(reader.GetInt32("tipo"))[0],
                    Marca = Marca.New.Get(reader.GetInt32("marca"))[0],
                    Code = reader.GetString("code"),
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
