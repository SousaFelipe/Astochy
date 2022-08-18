using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using VadenStock.Core.Http;

using VadenStock.Http;
using VadenStock.Tools;



namespace VadenStock.View.Components.Organisms
{
    public partial class ClienteItem : Border
    {
        private readonly Cliente cliente;



        public ClienteItem()
        {
            this.cliente = Cliente.Conn;

            InitializeComponent();
        }



        public ClienteItem(Cliente cliente)
        {
            this.cliente = cliente;

            InitializeComponent();

            Loaded += delegate
            {
                _ImageAccount.Source = StatusIcon();
                _TextNomeCliente.Text = this.cliente.razao;
                _TextEndereco.Text = EnderecoCompleto();

                RequestContratos();
            };
        }



        private async void RequestContratos()
        {
            Response response = await Contrato.Conn.Where("id_cliente", "=", cliente.id).Get(10);

            List<Contrato>? contratos = response.Registros.ToObject<List<Contrato>>();

            if (contratos != null && contratos.Count > 0)
                foreach (Contrato contrato in contratos)
                    _StackPlanos.Children.Add(new ContratoItem(contrato));
        }



        private ImageSource? StatusIcon()
        {
            return cliente.ativo.Equals("S")
                ? Src.Icon("green-account")
                : Src.Icon("red-account");
        }



        private string EnderecoCompleto()
        {
            string endereco = cliente.endereco;

            endereco += string.IsNullOrEmpty(cliente.numero)
                ? " - SN"
                : $" - {cliente.numero}";

            if (!string.IsNullOrEmpty(cliente.complemento))
                endereco += $", {cliente.complemento}";

            else if (!string.IsNullOrEmpty(cliente.referencia))
                endereco += $", {cliente.referencia}";

            return endereco;
        }
    }
}
