using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using VadenStock.View.Models;
using VadenStock.View.Structs;
using VadenStock.View.Components.Forms;

using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ProdutoDialog : Border
    {
        private readonly bool EditMode;
        private ProdutoStruct Produto;
        private ProdutoStruct EditarProduto;



        public ProdutoDialog()
        {
            EditMode = false;

            InitializeComponent();

            Loaded += delegate
            {
                LoadMarcas();
                LoadCategorias();
            };
        }



        public ProdutoDialog(ProdutoType produto)
        {
            Produto = new(produto);
            EditarProduto = Produto;
            EditMode = true;

            InitializeComponent();
            LoadTipos();
            LoadCategorias();
            LoadMarcas();

            Loaded += delegate
            {
                LoadProduto();
            };
        }



        void LoadTipos()
        {
            foreach (TipoType t in TiposViewModel.TiposPorCategoria(Produto.Categoria))
            {
                _ComboTipos.Items.Add(new ComboBoxItem()
                {
                    Tag = t.Id,
                    Content = t.Name
                });
            }
        }



        void LoadCategorias()
        {
            foreach (CategoriaType c in CategoriasViewModel.TodasAsCategorias)
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = c.Id,
                    Content = c.Name
                });
            }
        }



        void LoadMarcas()
        {
            foreach (MarcaType m in MarcasViewModel.TodasAsMarcas)
            {
                _ComboMarcas.Items.Add(new ComboBoxItem()
                {
                    Tag = m.Id,
                    Content = m.Name,
                });
            }
        }



        public void LoadProduto()
        {
            _BorderImage.Background = new ImageBrush() { ImageSource = Src.Storage($"{ EditarProduto.Image.FileName }{ EditarProduto.Image.FileExtension }") };
            _TextImageName.Text = EditarProduto.Image.FileName;
            _InputName.Text = EditarProduto.Name;
            _ComboMarcas.SelectedItem = _ComboMarcas.Find(Produto.Marca);
            _ComboCategorias.SelectedItem = _ComboCategorias.Find(EditarProduto.Categoria);
            _ComboTipos.SelectedItem = _ComboTipos.Find(EditarProduto.Tipo);
            _InputPrice.Text = Str.Currency(EditarProduto.Valor);
            _InputDescription.Text = EditarProduto.Description;
        }



        private void ShouldBeSaveEnabled()
        {
            if (_ButtonSave != null)
            {
                _ButtonSave.IsEnabled = (
                    Produto.Name != EditarProduto.Name ||
                    Produto.Categoria != EditarProduto.Categoria ||
                    Produto.Tipo != EditarProduto.Tipo ||
                    Produto.Marca != EditarProduto.Marca ||
                    Produto.Marca != EditarProduto.Marca ||
                    Produto.Image.Origin != EditarProduto.Image.Origin ||
                    Produto.Image.FileName != EditarProduto.Image.FileName ||
                    Produto.Description != EditarProduto.Description
               );
            }
        }



        private void SelectImageAvatar(string filePath)
        {
            BitmapImage? image = Src.OpenBitmap(filePath);

            if (image != null)
            {
                string fileNameFull = filePath.Split('\\')[^1];

                Produto.Image.FileExtension = Path.GetExtension(fileNameFull).ToLower();
                Produto.Image.FileName = fileNameFull.Replace(Produto.Image.FileExtension, "").ToLower();
                Produto.Image.Origin = filePath;

                _BorderImage.Background = new ImageBrush() { ImageSource = image };
                _BorderButtonClearAvatar.Visibility = Visibility.Visible;

                _TextImageName.Text = Produto.Image.FileName;
                _TextImageName.Focus();
                _TextImageName.SelectionStart = Produto.Image.FileName.Length;
            }
        }



        private void ClearImageAvatar()
        {
            _BorderImage.Background = new ImageBrush() { ImageSource = Src.Icon("blue-image-plus") };
            _BorderButtonClearAvatar.Visibility = Visibility.Collapsed;
            _TextImageName.Text = string.Empty;

            Produto.Image.FileExtension = string.Empty;
            Produto.Image.FileName = string.Empty;
            Produto.Image.Origin = string.Empty;
        }



        private void OpenImageDialog(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new()
            {
                DefaultExt = ".png",
                Filter = "Image documents (.png)|*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    SelectImageAvatar(dialog.FileName);
                }
            }
        }



        private void ButtonClearAvatar_Click(object sender, MouseButtonEventArgs e)
        {
            ClearImageAvatar();
        }



        private void InputImageName_Change(object sender, TextChangedEventArgs e)
        {
            InputTransparent input = (InputTransparent)sender;
            Produto.Image.FileName = input.Text;

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Produto.Name = input.Text;

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void SelectMarca_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
                Produto.Marca = Convert.ToInt32(item.Tag);
            else
                Produto.Marca = 0;

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void SelectCategoria_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                Produto.Categoria = Convert.ToInt32(item.Tag);

                if (!EditMode)
                    LoadTipos();
            }
            else
            {
                Produto.Categoria = 0;
                
                if (!EditMode)
                    _ComboTipos.Clear(true);
            }

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void SelectTipo_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
                Produto.Tipo = Convert.ToInt32(item.Tag);
            else
                Produto.Tipo = 0;

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void InputPrice_Changed(object sender, TextChangedEventArgs e)
        {
            InputCurrency input = (InputCurrency)sender;
            string? text = input.Text;
            
            if (!string.IsNullOrEmpty(text))
                Produto.Valor = Convert.ToDouble(text);

            System.Diagnostics.Trace.WriteLine(Produto.Valor);

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void InputDescription_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Produto.Description = input.Text;

            if (EditMode)
                ShouldBeSaveEnabled();
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(Produto.Image.FileName))
            {
                if (string.IsNullOrEmpty(Produto.Image.Origin))
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Adicione uma imagem ao produto"));

                else
                {
                    string fileNameFull = Produto.Image.Origin.Split('\\')[^1];

                    Produto.Image.FileExtension = Path.GetExtension(fileNameFull);
                    Produto.Image.FileName = fileNameFull.Replace(Produto.Image.FileExtension, "").ToLower();
                }
            }
            else if (string.IsNullOrEmpty(Produto.Name))
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Insira o nome do produto"));

            else if (Produto.Categoria <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar uma categoria"));

            else if (Produto.Tipo <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar o tipo de produto"));

            else if (Produto.Marca <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar a marca"));

            else if (Produto.Valor <= 0)
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "O preço do produto não pode ser 0,00"));

            else
            {
                if (Produto.Id > 0)
                    AtualizarProduto();

                else
                    SalvarProduto();
            }
        }



        private void SalvarProduto()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            bool saved = Src.CopyToResource(
                    Produto.Image.Origin,
                    $"{ Src.Resource.Root }\\{ Src.Resource.Storage }",
                    $"{ Produto.Image.FileName }{ Produto.Image.FileExtension }"
                );

            if (saved)
            {
                if (ProdutosViewModel.Create(Produto) > 0)
                {
                    ClearImageAvatar();

                    _InputName.Clear();
                    _ComboMarcas.SelectedIndex = 0;
                    _ComboCategorias.SelectedIndex = 0;
                    _ComboTipos.SelectedIndex = 0;
                    _InputPrice.Text = "0,00";
                    _InputDescription.Clear();

                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Produto salvo com sucesso"));
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Não foi possível salvar o Produto"));
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao copiar imagem para o sistema."));
        }



        private void AtualizarProduto()
        {
            bool saved = true;
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (ShouldBeChangeImage())
                saved = Src.CopyToResource(
                        Produto.Image.Origin,
                        $"{ Src.Resource.Root }\\{ Src.Resource.Storage }",
                        $"{ Produto.Image.FileName }{ Produto.Image.FileExtension }"
                    );

            if (saved)
            {
                if (ProdutosViewModel.Create(Produto) > 0)
                {
                    ClearImageAvatar();

                    _InputName.Clear();
                    _ComboMarcas.SelectedIndex = 0;
                    _ComboCategorias.SelectedIndex = 0;
                    _ComboTipos.SelectedIndex = 0;
                    _InputPrice.Text = "0,00";
                    _InputDescription.Clear();

                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Produto salvo com sucesso"));
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Não foi possível salvar o Produto"));
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao copiar imagem para o sistema"));
        }



        private bool ShouldBeChangeImage()
        {
            return !Produto.Image.FileName.Equals(
                    EditarProduto.Image.FileName + EditarProduto.Image.FileExtension
                );
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
