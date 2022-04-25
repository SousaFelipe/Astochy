using MySql.Data.MySqlClient;



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



        public virtual Connection Count(string column = "*")
        {
            Builder.Count(column);
            return this;
        }



        public virtual Connection Select(string[]? selects = null)
        {
            Builder.Select(selects);
            return this;
        }



        public virtual Connection Where(string column, string oper, object? value = null)
        {
            Builder.Where(column, oper, value);
            return this;
        }



        public int Bind(bool clear = true)
        {
            try
            {
                using (Plug = new MySqlConnection(ConnectionString))
                {
                    Plug.Open();

                    using (Cmmd = new MySqlCommand(Builder.SQL(clear), Plug))
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



        protected void Unplug()
        {
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
