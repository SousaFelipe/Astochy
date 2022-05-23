using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;



namespace VadenStock.Core
{
    public abstract class Connection
    {
        private static readonly string SERVER   = "localhost";
        private static readonly string USER_ID  = "free";
        private static readonly string PASSWORD = "12345678";
        private static readonly string DATABASE = "vaden_darth_schema";



        protected static string ConnectionString
        {
            get { return $"server={ SERVER };user id={ USER_ID };password={ PASSWORD };database={ DATABASE }"; }
        }


        
        protected MySqlConnection?   Plug   { get; set; }
        protected MySqlCommand?      Cmmd   { get; set; }
        protected MySqlDataReader?   Reader { get; set; }



        protected QueryBuilder Builder { get; private set; }



        protected Connection(string table)
        {
            Builder = new(table);
        }



        public virtual int Create(List<string[]> inserts, bool timestamps = true)
        {
            int output = -1;

            List<string> fields = new();
            List<string> values = new();

            foreach (string[] row in inserts)
            {
                fields.Add(row[0]);
                values.Add(row[1]);
            }

            if (timestamps)
            {
                fields.Add("created_at");
                values.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            using (Plug = new MySqlConnection(ConnectionString))
            {
                Plug.Open();

                using (Cmmd = new MySqlCommand(Builder.Create(fields), Plug))
                {
                    for (int i = 0; i < values.Count; i++)
                        Cmmd.Parameters.AddWithValue($"@{ fields[i] }", values[i]);

                    output = Cmmd.ExecuteNonQuery();
                }

                Unplug();
            }

            return output;
        }



        public virtual Connection Where(string column, object operOrValue, object? value = null)
        {
            Builder.Where(column, operOrValue, value);
            return this;
        }



        public virtual Connection Or(string column, object operOrValue, object? value = null)
        {
            Builder.Or(column, operOrValue.ToString(), value);
            return this;
        }



        public int Count(string column = "*")
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();
                    Builder.Count(column);

                    using (Cmmd = new MySqlCommand(Builder.Query, Plug))
                    {
                        return int.Parse(Cmmd.ExecuteScalar().ToString());
                    }
                }
            }
            finally
            {
                Unplug();
            }
        }



        public virtual bool Delete(int id)
        {
            bool output = false;

            using (Plug = new MySqlConnection(ConnectionString))
            {
                Plug.Open();

                using (Cmmd = new MySqlCommand($"DELETE FROM { Builder.Table } WHERE id=@id", Plug))
                {
                    Cmmd.Parameters.AddWithValue($"@id", id.ToString());
                    output = Cmmd.ExecuteNonQuery() > 0;
                }

                Unplug();
            }

            return output;
        }



        protected void Unplug()
        {
            if (Builder != null)
                Builder = new(Builder.Table);

            if (Plug != null)
                Plug.Close();

            if (Cmmd != null)
                Cmmd.Dispose();

            if (Reader != null)
            {
                Reader.Dispose();
                Reader.Close();
            }
        }
    }
}
