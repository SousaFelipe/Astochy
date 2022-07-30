using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;
using VadenStock.Tools;
using VadenStock.View.Components.Forms;
using VadenStock.View.Models;



namespace VadenStock.View.Dialogs
{
    public partial class FornecedorDialog : Border
    {
        private FornecedorType NewFornecedor;



        public FornecedorDialog()
        {
            NewFornecedor = new()
            {
                Cnpj = string.Empty,
                Fantasia = string.Empty,
                Email = string.Empty,
                Contato = string.Empty,
                Telefone = string.Empty,
                Whatsapp = string.Empty,
            };

            InitializeComponent();
        }



        public void ShouldBeSaveEnabled()
        {
            _ButtonSave.IsEnabled = (
                !string.IsNullOrEmpty(NewFornecedor.Fantasia) &&
                !string.IsNullOrEmpty(NewFornecedor.Telefone) &&
                NewFornecedor.Cnpj.Length >= 18 &&
                NewFornecedor.Telefone.Length >= 14
            );
        }



        private void InputFantasia_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string fantasia = input.Text;

            NewFornecedor.Fantasia = fantasia;

            ShouldBeSaveEnabled();
        }



        private void InputEmail_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string email = input.Text;

            NewFornecedor.Email = email;

            ShouldBeSaveEnabled();
        }



        private void InputCNPJ_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string cnpj = Str.CNPJ(input.Text);

            input.Text = cnpj;
            input.CaretIndex = input.Text.Length;

            NewFornecedor.Cnpj = cnpj;

            ShouldBeSaveEnabled();
        }



        private void InputContato_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string contato = Str.Phone(input.Text);

            input.Text = contato;
            input.CaretIndex = input.Text.Length;

            NewFornecedor.Contato = contato;
        }



        private void InputTelefone_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string phone = Str.Phone(input.Text);

            input.Text = phone;
            input.CaretIndex = input.Text.Length;

            NewFornecedor.Telefone = phone;

            ShouldBeSaveEnabled();
        }



        private void InputWhatsapp_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            string whatsapp = Str.Phone(input.Text);

            input.Text = whatsapp;
            input.CaretIndex = input.Text.Length;

            NewFornecedor.Whatsapp = whatsapp;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(NewFornecedor.Fantasia))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor insira o nome fantasia do fornecedor"));

            else if (string.IsNullOrEmpty(NewFornecedor.Cnpj) || NewFornecedor.Cnpj.Length < 18)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor insira o CNPJ corretamente"));

            else if (string.IsNullOrEmpty(NewFornecedor.Telefone))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Por favor preencha o campo \"Telefone\" corretamente"));

            else
            {
                FornecedorType? fornecedor = FornecedoresViewModel.Find(NewFornecedor.Cnpj);

                if (fornecedor != null)
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Este fornecedor já está cadastrado"));

                else if (FornecedoresViewModel.Create(NewFornecedor))
                {
                    _InputFantasia.Clear();
                    _InputEmail.Clear();
                    _InputCNPJ.Clear();
                    _InputContato.Clear();
                    _InputTelefone.Clear();
                    _InputWhatsapp.Clear();
                    _ButtonSave.IsEnabled = false;

                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Fornecedor cadastrado com sucesso"));
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Ocorreu um erro ao inserir o registro"));
            }
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
