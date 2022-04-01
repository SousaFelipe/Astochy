using System;

using MySql.Data.MySqlClient;



namespace VadenStock.Model
{
    public class Marca : Connection
    {
        public struct Contract
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
        };



        public Marca() : base("marcas") { }



        public Contract? Load(int id)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    using (Cmmd = new MySqlCommand(Builder.Load(id), Plug))
                    {
                        using (Reader = Cmmd.ExecuteReader())
                        {
                            if (Reader.Read())
                                return Content.Get(Reader);
                        }
                    }
                }
            }
            finally
            {
                Unplug();
            }

            return null;
        }



        private class Content
        {
            public static Contract Get(MySqlDataReader reader)
            {
                Contract contract = new()
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString("description"),
                    CreatedDate = reader.GetDateTime("created_at")
                };

                return contract;
            }
        }
    }
}
