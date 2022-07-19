using VadenStock.Core.Http;



namespace VadenStock.Http
{
    public partial class Cliente : IXCClient
    {
        public static readonly Cliente Conn = new();



        public int id;
        public string razao;



        public Cliente() : base("cliente")
        {
            id = 0;
            razao = string.Empty;
        }
    }
}
