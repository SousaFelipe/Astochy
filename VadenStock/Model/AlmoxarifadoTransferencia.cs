using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;



namespace VadenStock.Model
{
    public class AlmoxarifadoTransferencia : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public Item.Contract Item { get; set; }
            public Almoxarifado.Contract From { get; set; }
            public Almoxarifado.Contract? To { get; set; }
            public Item.Contract.Status Action { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
        }



        public AlmoxarifadoTransferencia New { get { return new AlmoxarifadoTransferencia(); } }



        public AlmoxarifadoTransferencia() : base("almoxarifados_transferencias") { }



        public List<Contract> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<Contract> list = new();
                    string? query = id > 0 ? Builder.Load(id) : Builder.Query;

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



        private class Content
        {
            public static Contract Get(MySqlDataReader reader)
            {
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Item = new Item().Get(reader.GetInt32("item"))[0],
                    From = new Almoxarifado().Get(reader.GetInt32("from_almoxarifado"))[0],
                    To = reader.IsDBNull(3) ? null : new Almoxarifado().Get(reader.GetInt32("to_almoxarifado"))[0],
                    Action = Item.Contract.GetStatus(reader.GetString("action")),
                    Description = reader.IsDBNull(5) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
