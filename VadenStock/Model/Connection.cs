using MySql.Data.MySqlClient;



namespace VadenStock.Model
{
    public abstract class Connection
    {
        private static readonly string SERVER   = "localhost";
        private static readonly string USER_ID  = "root";
        private static readonly string PASSWORD = "xv78#@90af";
        private static readonly string DATABASE = "vaden_stock";



        protected static string ConnectionString
        {
            get { return $"server={ SERVER };user id={ USER_ID };password={ PASSWORD };database={ DATABASE }"; }
        }


        
        protected MySqlConnection?   Plug   { get; set; }
        protected MySqlCommand?      Cmmd   { get; set; }
        protected MySqlDataReader?   Reader { get; set; }



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



        protected static string RawLoad(string table, int id)
        {
            return RawQuery(table, new string[] { "id" }, new string[] { id.ToString() });
        }



        protected static string RawQuery(string table, string[] cols, string[] values)
        {
            string query = $"SELECT * FROM { table } ";

            if (cols != null && values != null)
            {
                if (cols.Length == values.Length)
                {
                    query += "WHERE ";

                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] != null && values[i] != null)
                        {
                            query += (i < (cols.Length - 1))
                                    ? $"{ cols[i] }='{ values[i] }' AND "
                                    : $"{ cols[i] }='{ values[i] }'";
                        }
                    }
                }
            }

            return query;
        }
    }
}
