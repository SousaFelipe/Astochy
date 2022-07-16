using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

using VadenStock.Tools;



namespace VadenStock.Core.Http
{
    public class API
    {
        public struct Request
        {
            public string qtype;
            public object query;
            public string oper;
            public object page;
            public object rp;
            public string sortname;
            public string sortorder;

            public override string ToString()
            {
                return "{" +
                            $"\"qtype\": \"{qtype}\", " +
                            $"\"query\": \"{query}\", " +
                            $"\"oper\": \"{oper}\", " +
                            $"\"page\": \"{page}\", " +
                            $"\"rp\": \"{rp}\", " +
                            $"\"sortname\": \"{sortname}\", " +
                            $"\"sortorder\": \"{sortorder}\"" +
                       "}";
            }
        }


        
        public async static Task<string> SendRequest(string table, string column, string oper, string val)
        {
            using HttpClient httpClient = new();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "6:d94f8ccff332c49a266088ea3e0afaa2bdac77157bc4c698d7ab7e35971192bd".ToBase64());

            Request body = new()
            {
                qtype = $"{table}.{column}",
                query = val,
                oper = oper,
                page = 1,
                rp = 20,
                sortname = $"{table}.id",
                sortorder = "asc"
            };

            StringContent content = new(body.ToString(), Encoding.UTF8, "application/json");
            content.Headers.Add("ixcsoft", "listar");

            HttpResponseMessage response = await httpClient.PostAsync($"https://agilityquixeramobim.com.br/webservice/v1/{table}", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return json;
            }

            return response.StatusCode.ToString();
        }
    }
}
