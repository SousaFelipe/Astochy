using System;



namespace VadenStock.Model.Types
{
    public struct ConfigType : IModelType
    {
        public enum Protocol
        {
            HTTP,
            HTTPS,
        }



        public int Id { get; set; }
        public string ProductionPath { get; set; }
        public AlmoxType? AlmoxPrincipal { get; set; }
        public string ServerAddress { get; set; }
        public Protocol ServerProtocol { get; set; }
        public string ServerToken { get; set; }
        public DateTime CreatedDate { get; set; }



        public static Protocol GetProtocol(string protocol)
        {
            return protocol switch
            {
                "HTTP" => Protocol.HTTP,
                "HTTPS" => Protocol.HTTPS,
                _ => Protocol.HTTP,
            };
        }



        public static string GetServerProtocol(Protocol protocol)
        {
            return protocol switch
            {
                Protocol.HTTP => "http://",
                Protocol.HTTPS => "https://",
                _ => "http://"
            };
        }
    }
}
