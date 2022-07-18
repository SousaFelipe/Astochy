using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;

using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ConfigDialog : Border
    {
        private ConfigType Config;



        public ConfigDialog()
        {
            InitializeComponent();

            Loaded += delegate
            {
                Config = new()
                {
                    ProductionPath = Src.Resource.Root.Replace("\\Resources", "")
                };
            };
        }



        private void ShouldBeEnableSave()
        {
            if (_ButtonSave != null)
            {
                _ButtonSave.IsEnabled = (
                    _SelectProtocolo.SelectedIndex > 0 &&
                    !string.IsNullOrEmpty(Config.ServerAddress) &&
                    !string.IsNullOrEmpty(Config.ServerToken) &&
                    Config.ServerToken.Length == 66
                );
            }
        }



        private void SelectProtocolo_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            
            if (select.SelectedIndex > 0)
            {
                ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

                if (item != null && item.Tag != null)
                    Config.ServerProtocol = ConfigType.GetProtocol(item.Tag.ToString());
            }

            ShouldBeEnableSave();
        }



        private void InputAddress_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Config.ServerAddress = input.Text.Trim();
            ShouldBeEnableSave();
        }



        private void InputToken_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Config.ServerToken = input.Text.Trim();
            ShouldBeEnableSave();
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (ConfigsViewModel.Create(Config))
            {
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Configurações atualizadas com sucesso", "Yehooow!"));
                CloseDialog(sender, e);
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao salvar configurações"));
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
