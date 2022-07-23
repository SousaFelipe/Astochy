using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using VadenStock.Http;
using VadenStock.Tools;



namespace VadenStock.View.Components
{
    public partial class ClienteItem : Border
    {
        private Cliente Model;



        public ClienteItem()
        {
            Model = Cliente.Conn;

            InitializeComponent();
        }



        public ClienteItem(Cliente cliente)
        {
            Model = cliente;

            InitializeComponent();

            Loaded += delegate
            {
                _ImageAccount.Source = StatusIcon();
                _TextNomeCliente.Text = Model.razao;
                _TextEndereco.Text = EnderecoCompleto();
            };
        }



        private ImageSource? StatusIcon()
        {
            return Model.ativo
                ? Src.Icon("green-account")
                : Src.Icon("red-account");
        }



        private string EnderecoCompleto()
        {
            string endereco = Model.endereco;

            endereco += string.IsNullOrEmpty(Model.numero)
                ? " - SN"
                : $" - {Model.numero}";

            if (!string.IsNullOrEmpty(Model.complemento))
                endereco += $", {Model.complemento}";

            else if (!string.IsNullOrEmpty(Model.referencia))
                endereco += $", {Model.referencia}";

            return endereco;
        }
    }
}
