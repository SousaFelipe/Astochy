using System;
using System.Collections.Generic;



namespace VadenStock.Core.Http
{
    public class Payload
    {
        public string Table { get; private set; }



        private string qtype;
        private string? query;
        private string? oper;
        private int page;
        private int rp;
        private string sortname;
        private string sortorder;


        private int WhereCount = 0;
        private readonly Dictionary<string, string[]> grid = new();



        public Payload(string table)
        {
            Table = table;
            qtype = $"{Table}.id";
            query = "";
            oper = "=";
            page = 1;
            rp = 20;
            sortname = $"{Table}.id";
            sortorder = "asc";
        }



        private static string CompileObject(string obj, bool quotes = false)
        {
            string jsonObj = quotes
                ? "\"{[OBJ]}\""
                : "{[OBJ]}";

            return jsonObj.Replace("[OBJ]", obj);
        }



        private static string CompileArray(string array, bool quotes = false)
        {
            string jsonArray = quotes
                ? "\"[{ARRAY}]\""
                : "[{ARRAY}]";

            return jsonArray.Replace("{ARRAY}", array);
        }



        public void Where(string qtype, object oper, object query)
        {
            string strQtype = $"{ (qtype ?? "id") }";
            string strQuery = $"{ (query ?? "0") }";
            string strOper = $"{ (oper ?? "!=") }";

            if (WhereCount == 0)
            {
                this.qtype = $"{Table}.{strQtype}";
                this.query = strQuery;
                this.oper = strOper;
            }
            else if (!grid.ContainsKey(qtype))
                grid.Add(strQtype, new string[2] { strOper, strQuery });

            WhereCount++;
        }



        public void OrderBy(string column, string order)
        {
            sortname = $"{Table}.{column}";
            sortorder = order;
        }



        public Payload In(int page)
        {
            this.page = page;
            return this;
        }



        public Payload Max(int rp)
        {
            this.rp = rp;
            return this;
        }



        public override string ToString()
        {
            string payload = $"\"qtype\": \"{qtype}\", " +
                             $"\"query\": \"{query}\", " +
                             $"\"oper\": \"{oper}\", " +
                             $"\"page\": \"{page}\", " +
                             $"\"rp\": \"{rp}\", " +
                             $"\"sortname\": \"{sortname}\", " +
                             $"\"sortorder\": \"{sortorder}\"";

            if (grid.Count > 0)
            {
                payload += ", \"grid_param\": ";

                int count = 0;
                string grid = string.Empty;

                foreach (var row in this.grid)
                {
                    string current = $"\\\"TB\\\": \\\"{ Table }.{ row.Key }\\\", \\\"OP\\\": \\\"{ row.Value[0] }\\\", \\\"P\\\": \\\"{row.Value[1]}\\\"";

                    grid += (count < (this.grid.Count - 1))
                        ? string.Concat(CompileObject(current), ", ")
                        : CompileObject(current);
                    
                    count++;
                }

                payload = string.Concat(payload, CompileArray(grid, true));
            }

            WhereCount = 0;
            grid.Clear();

            return CompileObject(payload);
        }
    }
}
