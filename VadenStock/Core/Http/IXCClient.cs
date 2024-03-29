﻿using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.Core.Http
{
    public class IXCClient
    {
        public int id;
        public int id_ixc;
        public bool sync;
        public DateTime? updated_at;
        public DateTime created_at;



        private ConfigType DefaultConfigs;

        private readonly string     Table;
        private readonly HttpClient Client;
        private readonly Payload    Body;



        public IXCClient(string table)
        {
            id = 0;
            id_ixc = 0;
            sync = false;
            created_at = DateTime.Now;

            Table = table;
            Body = new Payload(Table);
            DefaultConfigs = Config.Model.Where("id", 1).Select()[0];
            Client = new HttpClient();
        }



        private string Url()
        {
            string protocol = ConfigType.GetServerProtocol(DefaultConfigs.ServerProtocol);
            string address = DefaultConfigs.ServerAddress;
            return $"{protocol}{address}/webservice/v1/{Table}";
        }



        private static StringContent Content(string body)
        {
            StringContent content = new(body, Encoding.UTF8, "application/json");
            content.Headers.Add("ixcsoft", "listar");
            return content;
        }



        public IXCClient Where(string column, object operOrValue, object? value = null)
        {
            string? realO = (value == null) ? "=" : $"{operOrValue}";
            string? realV = (value ?? operOrValue).ToString();

            Body.Where(column, realO, realV);

            return this;
        }



        public IXCClient OrderBy(string column, string order)
        {
            Body.OrderBy(column, order);
            return this;
        }



        public IXCClient In(int page)
        {
            Body.In(page);
            return this;
        }



        public async Task<Response> Get(int rowsPerPage = 0)
        {
            try
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", DefaultConfigs.ServerToken.ToBase64());

                string body = Body.Max(rowsPerPage > 0 ? rowsPerPage : 20).ToString();
                HttpResponseMessage response = await Client.PostAsync(Url(), Content(body)).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return new Response(json);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("[VADEN.ERROR] " + e.Message);
            }

            return new Response(string.Empty);
        }
    }
}
