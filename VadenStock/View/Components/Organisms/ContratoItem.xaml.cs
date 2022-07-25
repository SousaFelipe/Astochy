using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

using VadenStock.Core.Http;
using VadenStock.Http;
using VadenStock.Tools;



namespace VadenStock.View.Components.Organisms
{
    public partial class ContratoItem : Grid
    {
        private readonly Contrato contrato;



        public ContratoItem(Contrato contrato)
        {
            this.contrato = contrato;

            InitializeComponent();

            Loaded += delegate
            {
                LoadContrato();
                RequestLogins();
            };
        }



        private void LoadContrato()
        {
            _ImageContratoStatus.Source = Src.Icon(ContratoIconStatus());
            _TextContrato.Text = contrato.contrato;
        }



        public void RequestLogins()
        {
            Application.Current.Dispatcher.Invoke(async () => {

                Response response = await Login.Conn.Where("id_contrato", "=", contrato.id).Get(1);
                List<Login>? logins = response.Registros.ToObject<List<Login>>();

                if (logins != null && logins.Count > 0)
                {
                    _ImageLoginStatus.Source = Src.Icon(LoginIconStatus(logins[0]));
                    _TextLogin.Text = logins[0].login;
                    _TextMAC.Text = string.IsNullOrEmpty(logins[0].mac) ? "" : $"MAC: {logins[0].mac}";
                }
            });
        }



        public string ContratoIconStatus()
        {
            return contrato.GetStatus() switch
            {
                Contrato.Status.Pre => "blue-file",
                Contrato.Status.Ativo => "green-check",
                Contrato.Status.Inativo => "gray-file",
                Contrato.Status.Negativado => "red-file",
                Contrato.Status.Desistiu => "gray-file",
                _ => "gray-file"
            };
        }



        public static string LoginIconStatus(Login login)
        {
            return login.online switch
            {
                "S" => "green-wifi",
                "SS" => "gray-wifi",
                "N" => "red-wifi",
                _ => "gray-wifi"
            };
        }
    }
}
