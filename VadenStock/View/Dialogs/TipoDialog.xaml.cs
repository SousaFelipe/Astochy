using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.View.Models;
using VadenStock.View.Structs;
using VadenStock.View.Components.Forms;



namespace VadenStock.View.Dialogs
{
    public partial class TipoDialog : Border
    {
        private TipoStruct Tipo;



        public TipoDialog()
        {
            Tipo = new();

            InitializeComponent();

            Loaded += delegate
            {
                LoadCategorias();
            };
        }



        private bool LoadCategorias()
        {
            if (_ComboCategorias != null)
            {
                if (_ComboCategorias.Items.Count > 1)
                    _ComboCategorias.Clear(true);

                foreach (CategoriaType c in CategoriasViewModel.TodasAsCategorias)
                {
                    _ComboCategorias.Items.Add(new ComboBoxItem()
                    {
                        Tag = c.Id.ToString(),
                        Content = c.Name
                    });
                }
            }

            return true;
        }



        private void ComboCategorias_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
                Tipo.Categoria = Convert.ToInt32(item.Tag);
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Tipo.Name = input.Text;
        }



        private void InputDescription_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Tipo.Description = input.Text;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (Tipo.Categoria <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar a Categoria!", "Oops!"));

            else if (string.IsNullOrEmpty(Tipo.Name))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você esqueceu de inseir o nome do Tipo!", "Oops!"));

            else if (TiposViewModel.Create(Tipo) > 0)
            {
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Registro inserido com sucesso!", "Yhow!"));
                ClearForm();
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao inserir registro"));
        }



        private void ClearForm()
        {
            _ComboCategorias.SelectedIndex = 0;
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
