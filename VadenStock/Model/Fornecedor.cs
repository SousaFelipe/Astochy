using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Fornecedor : Connection
    {
        public static Fornecedor Model { get { return new Fornecedor(); } }



        public Fornecedor() : base("fornecedores") { }



        public override Fornecedor Where(string column, object operOrValue, object? value = null)
        {
            return (Fornecedor)base.Where(column, operOrValue, value);
        }



        public List<FornecedorType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<FornecedorType> list = new();

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
