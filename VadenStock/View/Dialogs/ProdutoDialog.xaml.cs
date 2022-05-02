using System;
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
        private ProdutoStruct Produto;



        public ProdutoDialog()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadMarcas();
                LoadCategorias();
            };
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



        void LoadCategorias()
        {
            foreach(CategoriaType c in CategoriasViewModel.TodasAsCategorias)
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = c.Id,
                    Content = c.Name
                });
            }
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



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
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



        private void InputImageName_Change(object sender, TextChangedEventArgs e)
        {
            InputTransparent input = (InputTransparent)sender;
            Produto.Image.FileName = input.Text;
        }



        private void ButtonClearAvatar_Click(object sender, MouseButtonEventArgs e)
        {
            ClearImageAvatar();
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Produto.Name = input.Text;
        }



        private void SelectMarca_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
                Produto.Marca = Convert.ToInt32(item.Tag);
            else
                Produto.Marca = 0;
        }



        private void SelectCategoria_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
            {
                Produto.Categoria = Convert.ToInt32(item.Tag);
                LoadTipos();
            }
            else
            {
                Produto.Categoria = 0;
                _ComboTipos.Clear(true);
            }
        }



        private void SelectTipo_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            ComboBoxItem item = (ComboBoxItem)select.SelectedItem;

            if (item != null)
                Produto.Tipo = Convert.ToInt32(item.Tag);
            else
                Produto.Tipo = 0;
        }



        private void InputPrice_Changed(object sender, TextChangedEventArgs e)
        {
            InputCurrency input = (InputCurrency)sender;
            Produto.Price = Convert.ToDecimal(input.Text);
        }



        private void InputDescription_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Produto.Description = input.Text;
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = (MainWindow)Application.Current.MainWindow;

            if (string.IsNullOrEmpty(Produto.Image.FileName))
            {
                if (string.IsNullOrEmpty(Produto.Image.Origin))
                    owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Adicione uma imagem ao produto!"));

                else
                {
                    string fileNameFull = Produto.Image.Origin.Split('\\')[^1];

                    Produto.Image.FileExtension = Path.GetExtension(fileNameFull);
                    Produto.Image.FileName = fileNameFull.Replace(Produto.Image.FileExtension, "").ToLower();
                }
            }
            else if (string.IsNullOrEmpty(Produto.Name))
                owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Insira o nome do produto"));

            else if (Produto.Categoria <= 0)
                owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar uma categoria"));

            else if (Produto.Tipo <= 0)
                owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar o tipo de produto"));

            else if (Produto.Marca <= 0)
                owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "Você precisa selecionar a marca"));

            else if (Produto.Price <= 0)
                owner.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Warning, "O preço do produto não pode ser 0,00"));

            else
            {
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

                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Success, "Produto salvo com sucesso", "Wooow!"));
                }
                else
                    window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Não foi possível salvar o Produto"));
            }
            else
                window.DisplayAlert(new AlertDialog(AlertDialog.AlertType.Danger, "Erro ao copiar imagem para o sistema."));
        }
    }
}
