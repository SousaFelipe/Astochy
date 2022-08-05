using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Compra : Connection
    {
        public static Compra Model { get { return new Compra(); } }



        public Compra() : base("compras") { }



        public override Compra Where(string column, object operOrValue, object? value = null)
        {
            return (Compra)base.Where(column, operOrValue, value);
        }



        public override Compra Or(string column, object operOrValue, object? value = null)
        {
            return (Compra)base.Or(column, operOrValue, value);
        }



        public List<CompraType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<CompraType> list = new();

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



        public List<CompraType> OrderBy(string column, string order)
        {
            Builder.OrderBy(column, order);
            return Select();
        }



        private class Content
        {
            public static CompraType Get(MySqlDataReader reader)
            {
                CompraType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Fornecedor = Fornecedor.Model.Where("id", reader.GetInt32("fornecedor")).Select()[0],
                    NumSerie = reader.IsDBNull(2) ? string.Empty : reader.GetString("ns"),
                    ValorTotal = reader.GetDouble("valor_total"),
                    DataEmissao = reader.IsDBNull(4) ? null : reader.GetDateTime("data_emissao"),
                    Status = CompraType.GetStatus(reader.GetString("status")),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime("updated_at"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
