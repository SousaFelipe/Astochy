using System;
using VadenStock.Core;
using VadenStock.Core.Http;



namespace VadenStock.Http
{
    public class Contrato : IXCClient
    {
        public enum Tipo
        {
            Internet,
            Telefonia,
            Servicos,
            SVA
        }

        public enum Status
        {
            Pre,
            Ativo,
            Inativo,
            Negativado,
            Desistiu
        }

        public enum Acesso
        {
            Ativo,
            Desativado,
            BloqueioManual,
            BloqueioAutomatico,
            EmAtrasado,
            AguardandoAssunatura
        }



        public static readonly Contrato Conn = new();



        public string tipo;
        public int id_cliente;
        public int id_vd_contrato;
        public string contrato;
        public int id_tipo_contrato;
        public int id_modelo;
        public int id_filial;
        public string data_ativacao;
        public string data;
        public string dica_data_base;
        public string? data_renovacao;
        public string? pago_ate_data;
        public string status;
        public string status_internet;
        public int id_tipo_documento;
        public int id_carteira_cobranca;
        public int id_vendedor;
        public string? cc_previsao;
        public string? tipo_cobranca;
        public bool renovacao_automatica;
        public bool bloqueio_automatico;
        public bool aviso_atraso;
        public string? data_cancelamento;



        public Contrato() : base("cliente_contrato")
        {
            tipo = string.Empty;
            contrato = string.Empty;
            data_ativacao = string.Empty;
            data = string.Empty;
            dica_data_base = string.Empty;
            status = string.Empty;
            status_internet = string.Empty;
        }



        public Tipo GetTipo()
        {
            return tipo switch
            {
                "I" => Tipo.Internet,
                "T" => Tipo.Telefonia,
                "S" => Tipo.Servicos,
                "SVA" => Tipo.SVA,
                _ => Tipo.Internet
            };
        }



        public Status GetStatus()
        {
            return status switch
            {
                "P" => Status.Pre,
                "A" => Status.Ativo,
                "I" => Status.Inativo,
                "N" => Status.Negativado,
                "D" => Status.Desistiu,
                _ => Status.Pre
            };
        }



        public Acesso GetAcesso()
        {
            return status_internet switch
            {
                "A" => Acesso.Ativo,
                "D" => Acesso.Desativado,
                "CM" => Acesso.BloqueioManual,
                "CA" => Acesso.BloqueioAutomatico,
                "FA" => Acesso.EmAtrasado,
                "AA" => Acesso.AguardandoAssunatura,
                _ => Acesso.Desativado
            };
        }
    }
}
