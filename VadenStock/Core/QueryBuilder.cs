using System;
using System.Collections.Generic;



namespace VadenStock.Core
{
    public sealed class QueryBuilder
    {
        public string Table { get; private set; }
        public string Query { get; private set; }



        private int WC = 0;



        public QueryBuilder(string table)
        {
            Table = table;
            Query = "[ACTION] ";
            WC = 0;
        }



        public string Create(List<string> inserts)
        {
            Query = $"INSERT INTO { Table } (";

            for (int i = 0; i < inserts.Count; i++)
            {
                Query += (i < (inserts.Count - 1))
                    ? $"{ inserts[i] }, "
                    : $"{ inserts[i] })";
            }

            Query += " VALUES (";

            for (int i = 0; i < inserts.Count; i++)
            {
                Query += (i < (inserts.Count - 1))
                    ? $"@{ inserts[i] }, "
                    : $"@{ inserts[i] })";
            }

            return Query;
        }



        public void Select(string[]? selects = null)
        {
            string action = "SELECT ";

            if (selects?.Length > 0)
            {
                for (int s = 0; s < selects.Length; s++)
                {
                    action += (s < (selects.Length - 1))
                        ? $"{ selects[s] }, "
                        : $"{ selects[s] } ";
                }
            }
            else
            {
                action += "* ";
            }

            action += $"FROM { Table }";

            Query = Query.Replace("[ACTION]", action);
        }



        public void Count(string column = "*")
        {
            string action = $"SELECT COUNT({ column }) FROM { Table }";
            Query = Query.Replace("[ACTION]", action);
        }



        public QueryBuilder InnnerJoin(string table1, string column1, string? table2 = null, string column2 = "id")
        {
            Query += $" INNER JOIN { table1 } ON { table1 }.{ column2 }={ table2 ?? Table }.{ column1 }";
            return this;
        }



        public void Where(string column, object operOrValue, object? value = null)
        {
            object realO = value == null ? "=" : operOrValue;
            object realV = value ?? operOrValue;

            Query += (WC > 0)
                ? $" AND { column }{ realO }'{ realV }'"
                : $"WHERE { column }{ realO }'{ realV }'";

            WC += 1;
        }



        public void Or(string column, object operOrValue, object? value = null)
        {
            object realO = value == null ? "=" : operOrValue;
            object realV = value ?? operOrValue;

            Query += $" OR { column }{ realO }'{ realV }'";
        }



        public string? SQL(bool clear = true)
        {
            if (clear)
                WC = 0;

            return Query;
        }



        public string Load(int id)
        {
            return Raw(new string[] { "id" }, new string[] { id.ToString() });
        }



        public string Raw(string[] cols, string[] values)
        {
            Query = $"SELECT * FROM { Table }";

            if (cols.Length == values.Length)
            {
                Query += " WHERE ";

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

            return Query;
        }
    }
}
