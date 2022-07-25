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



        public void RequestLogins()
        {
            Task.Run(async () =>
            {
                Response response = await Login.Conn.Where("id_contrato", "=", contrato.id_ixc).Get(1);
                List<Login>? logins = response.Registros.ToObject<List<Login>>();

                if (logins != null && logins.Count > 0)
                {
                    _ImageLoginStatus.Source = Src.Icon(LoginIconStatus(logins[0]));
                    _TextMAC.Text = string.IsNullOrEmpty(logins[0].mac) ? "" : $"MAC: {logins[0].mac}";
                }
            });
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
