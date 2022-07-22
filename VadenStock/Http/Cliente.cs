using System;
using VadenStock.Core;
using VadenStock.Core.Http;



namespace VadenStock.Http
{
    public partial class Cliente : IXCClient
    {
        public enum TipoPessoa
        {
            Fisica,
            Juridica,
            Estrangeira
        }

        public enum TipoZona
        {
            Rural,
            Urbana
        }



        public static readonly Cliente Conn = new();



        public int id;
        public int id_ixc;
        public string razao;
        public int tipo_assinante;
        public TipoPessoa tipo_pessoa;
        public string? cnpj_cpf;
        public string? ie_identidade;
        public bool contribuinte_icms;
        public DateTime? data_nascimento;
        public bool ativo;
        public int filial_id;
        public string cep;
        public string endereco;
        public string numero;
        public string? complemento;
        public string bairro;
        public int cidade;
        public string? referencia;
        public TipoZona tipo_localidade;
        public string? telefone_comercial;
        public string? telefone_celular;
        public string? whatsapp;
        public string? obs;
        public string? alerta;
        public bool sync;
        public DateTime? updated_at;
        public DateTime created_at;



        public Cliente() : base("cliente")
        {
            id = 0;
            id_ixc = 0;
            razao = string.Empty;
            tipo_assinante = 0;
            tipo_pessoa = TipoPessoa.Fisica;
            filial_id = 0;
            cep = string.Empty;
            endereco = string.Empty;
            numero = "SN";
            bairro = string.Empty;
            cidade = 0;
            tipo_localidade = TipoZona.Rural;
            sync = false;
            created_at = DateTime.Now;
        }
    }
}
