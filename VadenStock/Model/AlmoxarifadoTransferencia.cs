using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class AlmoxarifadoTransferencia : Connection
    {
        public static AlmoxarifadoTransferencia Model { get { return new AlmoxarifadoTransferencia(); } }



        public AlmoxarifadoTransferencia() : base("almoxarifados_transferencias") { }



        public override AlmoxarifadoTransferencia Where(string column, object operOrValue, object? value = null)
        {
            return (AlmoxarifadoTransferencia)base.Where(column, operOrValue, value);
        }



        public List<TransfType> Select(params string[] selects)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Select(selects);

                    List<TransfType> list = new();

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
            public static TransfType Get(MySqlDataReader reader)
            {
                TransfType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Itens = reader.GetString("itens"),
                    From = Almoxarifado.Model.Where("id", reader.GetInt32("from_almoxarifado")).Select()[0],
                    To = Almoxarifado.Model.Where("id", reader.GetInt32("to_almoxarifado")).Select()[0],
                    Acao = ItemType.GetStatus(reader.GetString("acao")),
                    Description = reader.IsDBNull(5) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
