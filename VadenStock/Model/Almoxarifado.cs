﻿using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using VadenStock.Core;



namespace VadenStock.Model
{
    public class Almoxarifado : Connection
    {
        public struct Contract
        {
            public enum ETipo
            {
                Estoque,
                Carro,
                Moto,
                Indefinido
            }


            public int Id { get; set; }
            public ETipo Tipo { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }


            public static ETipo GetTipo(string tipo)
            {
                return tipo switch
                {
                    "E" => ETipo.Estoque,
                    "C" => ETipo.Carro,
                    "M" => ETipo.Moto,
                    _ => ETipo.Indefinido
                };
            }
        }



        public static Almoxarifado New { get { return new Almoxarifado(); } }



        public Almoxarifado () : base("almoxarifados") { }



        public List<Contract> Get(int id = 0)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    List<Contract> list = new();

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



        private class Content
        {
            public static Contract Get(MySqlDataReader reader)
            {
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Tipo = Contract.GetTipo(reader.GetString("tipo")),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
