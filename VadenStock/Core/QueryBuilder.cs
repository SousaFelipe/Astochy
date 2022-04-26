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



        public QueryBuilder Count(string column = "*")
        {
            Query = $"SELECT COUNT({ column }) FROM { Table } ";
            return this;
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



        public QueryBuilder InnnerJoin(string table, string joinOne, string joinTwo = "id")
        {
            Query += $"INNER JOIN { table } ON { table }.{ joinOne }={ Table }.{ joinTwo }";
            return this;
        }



        public QueryBuilder Where(string column, string oper, object? value = null)
        {
            object realOpera = value == null ? "=" : oper;
            object realValue = value ?? oper;

            Query += (WhereCount > 1)
                ? $" AND { Table }.{ column }{ realOpera }'{ realValue }'"
                : $"WHERE { Table }.{ column }{ realOpera }'{ realValue }'";

            WhereCount += 1;

            return this;
        }



        public string? SQL(bool clear = true)
        {
            if (clear)
                WhereCount = 0;

            return Query;
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
