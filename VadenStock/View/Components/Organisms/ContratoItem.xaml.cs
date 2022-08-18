using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using VadenStock.Core.Http;
using VadenStock.Http;
using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Dialogs;
using VadenStock.View.Models;



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

                Response response = await Login.Conn.Where("id_contrato", "=", contrato.id).Get(50);
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



        private void TextMAC_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock)sender;

            block.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0094CC"));
            block.TextDecorations = TextDecorations.Underline;
        }



        private void TextMAC_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock)sender;

            block.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#607D8B"));
            block.TextDecorations = null;
        }



        private void TextMAC_Click(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = (TextBlock)sender;
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            string mac = block.Text.Replace("MAC: ", "").Replace(":", "").Trim();
            ItemType? item = ItensViewModel.Find(mac);

            if (item != null)
                window.DisplayDialog(new HistoricoDialog(item.Value));
            
            else
            {
                ConfirmDialog dialog = new(ConfirmDialog.ConfirmType.Question, "Este item ainda não está no sistema.\nDeseja cadastrar?");

                window.DisplayDialog(
                        dialog.OnConfirm(sender => {
                            window.DisplayDialog(new EntradaDialog(new ItemType { Mac = mac }));
                            dialog.Close();
                        })
                );
            }
        }
    }
}
