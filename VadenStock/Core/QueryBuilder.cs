using System;
using System.Collections.Generic;



namespace VadenStock.Core
{
    public sealed class QueryBuilder
    {
        private string Table { get; set; }
        private int WhereCount { get; set; }
        public string? Query { get; private set; }



        public QueryBuilder(string table)
        {
            Table = table;
        }



        public QueryBuilder Select(string[]? selects = null)
        {
            Query = "SELECT ";

            if (selects?.Length > 0)
            {
                for (int s = 0; s < selects.Length; s++)
                {
                    Query += (s < (selects.Length - 1))
                        ? $"{ selects[s] }, "
                        : $"{ selects[s] } ";
                }
            }
            else
            {
                Query += "* ";
            }

            Query += $"FROM { Table } ";

            return this;
        }



        public QueryBuilder Where(string column, string oper, object value)
        {
            WhereCount += 1;

            Query += (WhereCount > 1)
                ? $" AND { column }{ oper }{ value }"
                : $"WHERE { column }{ oper }{ value }";

            return this;
        }



        public string Load(int id)
        {
            return Raw(new string[] { "id" }, new string[] { id.ToString() });
        }



        public string Raw(string[] cols, string[] values)
        {
            Query = $"SELECT * FROM { Table } ";

            if (cols != null && values != null)
            {
                if (cols.Length == values.Length)
                {
                    Query += "WHERE ";

                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] != null && values[i] != null)
                        {
                            Query += (i < (cols.Length - 1))
                                    ? $"{ cols[i] }='{ values[i] }' AND "
                                    : $"{ cols[i] }='{ values[i] }'";
                        }
                    }
                }
            }

            return Query;
        }
    }
}
