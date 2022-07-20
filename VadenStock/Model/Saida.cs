using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class Saida : Connection
    {
        public static Saida Model { get { return new Saida(); } }



        public Saida() : base("saidas") { }



        public override Saida Where(string column, object operOrValue, object? value = null)
        {
            return (Saida)base.Where(column, operOrValue, value);
        }



        public List<SaidaType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<SaidaType> list = new();

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
            public static SaidaType Get(MySqlDataReader reader)
            {
                SaidaType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Transferencia = AlmoxarifadoTransferencia.Model.Where("id", reader.GetInt32("transferencia")).Select()[0],
                    Responsavel = reader.GetString("responsavel"),
                    Tipo = ItemType.GetStatus(reader.GetString("tipo")),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
