using MySql.Data.MySqlClient;

using VadenStock.Core;



namespace VadenStock.Model
{
    public abstract class Connection
    {
        private static readonly string SERVER   = "localhost";
        private static readonly string USER_ID  = "root";
        private static readonly string PASSWORD = "xv78#@90af";
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



        public Connection Select(string[]? selects = null)
        {
            Builder.Select(selects);

            return this;
        }



        public Connection Where(string column, string oper, object value)
        {
            Builder.Where(column, oper, value);

            return this;
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
