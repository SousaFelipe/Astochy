using System;
using VadenStock.Core;
using VadenStock.Core.Http;



namespace VadenStock.Http
{
    public class Login : IXCClient
    {
        public enum TipoAutenticaco
        {
            PPPoE,
            Hotspot,
            IPxMAC,
            Vlan,
            IPoE,
            Integracao
        }



        public static readonly Login Conn = new();



        public string autenticacao;
        public int id_cliente;
        public int id_filial;
        public string login;
        public bool senha_md5;
        public string senha;
        public int login_simultaneo;
        public bool ativo;
        public bool online;
        public string ip;
        public string auto_preencher_ip;
        public string fixar_ip;
        public string relacionar_ip_ao_login;
        public string? mac;
        public string autenticacao_por_mac;
        public string auto_preencher_mac;
        public string relacionar_mac_ao_login;
        public int id_concentrador;
        public string? concentrador;
        public string? conexao;
        public string? tipo_conexao;
        public string tipo_vinculo_plano;
        public string? latitude;
        public string? longitude;



        public Login() : base("radusuarios")
        {
            autenticacao = string.Empty;
            login = string.Empty;
            senha = string.Empty;
            ip = string.Empty;
            auto_preencher_ip = string.Empty;
            fixar_ip = string.Empty;
            relacionar_ip_ao_login = string.Empty;
            autenticacao_por_mac = string.Empty;
            auto_preencher_mac = string.Empty;
            relacionar_mac_ao_login = string.Empty;
            tipo_vinculo_plano = string.Empty;
        }



        public TipoAutenticaco GetAutenticacao()
        {
            return autenticacao switch
            {
                "L" => TipoAutenticaco.PPPoE,
                "H" => TipoAutenticaco.Hotspot,
                "M" => TipoAutenticaco.IPxMAC,
                "V" => TipoAutenticaco.Vlan,
                "D" => TipoAutenticaco.IPoE,
                "I" => TipoAutenticaco.Integracao,
                _ => TipoAutenticaco.PPPoE
            };
        }
    }
}
