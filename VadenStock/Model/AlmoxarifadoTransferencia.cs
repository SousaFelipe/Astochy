using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;
using VadenStock.Model.Types;



namespace VadenStock.Model
{
    public class AlmoxarifadoTransferencia : Connection
    {
        public AlmoxarifadoTransferencia New { get { return new AlmoxarifadoTransferencia(); } }



        public AlmoxarifadoTransferencia() : base("almoxarifados_transferencias") { }



        public List<AlmoxTransfType> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<AlmoxTransfType> list = new();

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



        public override AlmoxarifadoTransferencia Count(string column = "*")
        {
            return (AlmoxarifadoTransferencia)base.Count(column);
        }



        public override AlmoxarifadoTransferencia Select(string[]? selects = null)
        {
            return (AlmoxarifadoTransferencia)base.Select(selects);
        }



        public override AlmoxarifadoTransferencia Where(string column, string oper, object? value = null)
        {
            return (AlmoxarifadoTransferencia)base.Where(column, oper, value);
        }



        private class Content
        {
            public static AlmoxTransfType Get(MySqlDataReader reader)
            {
                AlmoxTransfType contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Item = Item.New.Get(reader.GetInt32("item"))[0],
                    From = Almoxarifado.New.Get(reader.GetInt32("from_almoxarifado"))[0],
                    To = reader.IsDBNull(3) ? null : Almoxarifado.New.Get(reader.GetInt32("to_almoxarifado"))[0],
                    Action = ItemType.GetStatus(reader.GetString("action")),
                    Description = reader.IsDBNull(5) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
