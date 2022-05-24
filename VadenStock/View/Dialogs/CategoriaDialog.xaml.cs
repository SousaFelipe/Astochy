using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Tools;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Components;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Containers;



namespace VadenStock.View.Dialogs
{
    public partial class CategoriaDialog : Border
    {
        private CategoriaType Categoria;



        public CategoriaDialog()
        {
            Categoria = new();

            InitializeComponent();
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Categoria.Name = input.Text;
        }



        private void InputDescription_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Categoria.Description = input.Text;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(Categoria.Name))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de inseir o nome da categoria!", "Oops!"));

            else if (CategoriasViewModel.Create(Categoria) > 0)
            {
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Registro inserido com sucesso!", "Yhow!"));
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
