using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Compra : Connection
    {
        public static Compra New { get { return new Compra(); } }



        public Compra() : base("compras") { }



        public List<CompraType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<CompraType> list = new();

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
            public static CompraType Get(MySqlDataReader reader)
            {
                CompraType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Fornecedor = Fornecedor.New.Get(reader.GetInt32("fornecedor"))[0],
                    NumSerie = reader.GetString("ns"),
                    ValorTotal = reader.GetDouble("valor_total"),
                    DataEmissao = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime("data_emissao"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
