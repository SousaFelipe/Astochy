using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;



namespace VadenStock.View.Dialogs
{
    public partial class MarcaDialog : Border
    {
        private MarcaType Marca;



        public MarcaDialog()
        {
            Marca = new();

            InitializeComponent();
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Marca.Name = input.Text;
        }



        private void InputDescription_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Marca.Description = input.Text;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(Marca.Name))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de inseir o nome da Marca"));

            else if (MarcasViewModel.Create(Marca) > 0)
            {
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Registro inserido com sucesso"));
                ClearForm();
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao inserir registro"));
        }



        private void ClearForm()
        {
            _InputName.Text = string.Empty;
            _InputDescription.Text = string.Empty;
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
