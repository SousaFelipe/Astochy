using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Fornecedor : Connection
    {
        public static Fornecedor New { get { return new Fornecedor(); } }



        public Fornecedor() : base("fornecedores") { }



        public List<FornecedorType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<FornecedorType> list = new();

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



        public override Compra Count(string column = "*")
        {
            return (Compra)base.Count(column);
        }



        public override Compra Select(string[]? selects = null)
        {
            return (Compra)base.Select(selects);
        }



        public override Compra Where(string column, string operOrValue, object? value = null)
        {
            return (Compra)base.Where(column, operOrValue, value);
        }



        public override Compra InnerJoin(string table, string joinOne, string joinTwo = "id")
        {
            return (Compra)base.InnerJoin(table, joinOne, joinTwo);
        }



        private class Content
        {
            public static FornecedorType Get(MySqlDataReader reader)
            {
                FornecedorType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Cnpj = reader.GetString("cnpj"),
                    Fantasia = reader.GetString("fantasia"),
                    Email = reader.IsDBNull(3) ? string.Empty : reader.GetString("email"),
                    Contato = reader.IsDBNull(4) ? string.Empty : reader.GetString("contato"),
                    Telefone = reader.GetString("telefone"),
                    Whatsapp = reader.IsDBNull(6) ? string.Empty : reader.GetString("whatsapp"),
                    CreatedDate = reader.IsDBNull(7) ? System.DateTime.MinValue : reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
