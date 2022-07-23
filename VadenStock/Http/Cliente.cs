using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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



        public string razao;
        public int tipo_assinante;
        public string tipo_pessoa;
        public string? cnpj_cpf;
        public string? ie_identidade;
        public bool contribuinte_icms;
        public string? data_nascimento;
        public bool ativo;
        public int filial_id;
        public string cep;
        public string endereco;
        public string numero;
        public string? complemento;
        public string bairro;
        public int cidade;
        public string? referencia;
        public string tipo_localidade;
        public string? telefone_comercial;
        public string? telefone_celular;
        public string? whatsapp;
        public string? obs;
        public string? alerta;



        public Cliente() : base("cliente")
        {
            tipo_pessoa = string.Empty;
            razao = string.Empty;
            cep = string.Empty;
            endereco = string.Empty;
            numero = string.Empty;
            bairro = string.Empty;
            tipo_localidade = string.Empty;
        }



        public List<Contrato> RequestContratos()
        {
            List<Contrato>? contratos = new();

            Task.Run(async () =>
            {
                Response response = await Contrato.Conn
                    .Where("id_cliente", "=", id_ixc)
                    .Get();

                contratos = response.Registros.ToObject<List<Contrato>>();
            });

            return contratos ?? new List<Contrato>();
        }



        public List<Login> RequestLogin()
        {
            List<Login>? logins = new();

            Task.Run(async () =>
            {
                Response response = await Login.Conn
                    .Where("id_cliente", "=", id_ixc)
                    .Get();

                logins = response.Registros.ToObject<List<Login>>();
            });

            return logins ?? new List<Login>();
        }
    }
}
