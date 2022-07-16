using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace VadenStock.Core.Http
{
    public class Response
    {
        public JArray Registros { get; private set; }



        public Response(string response)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(response);
            Registros = result?.registros ?? new JArray();
        }
    }
}
