using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;

using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View.Dialogs
{
    public partial class ProdutoDialog : Window
    {
        struct ProdutoType
        {
            public string Name;
            public int Categoria;
            public int Tipo;
            public int Marca;
            public string ImageOrigin;
            public string Image;
            public decimal Price;
            public string Description;
        }



        private ProdutoType Produto;



        public ProdutoDialog()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadCategorias();
            };
        }



        void LoadCategorias()
        {
            _ComboCategorias.Clear(true);
            
            foreach(CategoriaType c in CategoriasViewModel.GetCategorias())
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = c.Id,
                    Content = c.Name
                });
            }
        }



        void LoadTipos(int categoria)
        {

        }



        void LoadMarcas()
        {

        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void OnClosing(object sender, CancelEventArgs e)
        {
            VadenStock.MainWindow window = (VadenStock.MainWindow)Application.Current.MainWindow;
            window.ExitDialogMode();
        }



        private void OpenImageDialog(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new()
            {
                DefaultExt = ".png",
                Filter = "Image documents (.png)|*.png"
            };

            if (true == dialog.ShowDialog())
            {
                if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    BitmapImage? image = Src.OpenBitmap(dialog.FileName);

                    if (image != null)
                    {
                        string fileName = dialog.FileName.Split('\\')[^1];

                        _BorderImage.Background = new ImageBrush() { ImageSource = image };
                        _TextImageName.Text = fileName.Replace(Path.GetExtension(fileName),  "");

                        Produto.Image = fileName;
                        Produto.ImageOrigin = dialog.FileName;
                    }
                }
            }
        }



        private void InputName_Changed(object sender, TextChangedEventArgs e)
        {
            InputText input = (InputText)sender;
            Produto.Name = input.Text;
        }



        private void SelectCategoria_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            object tag = ((ComboBoxItem)select.SelectedItem).Tag;

            Produto.Categoria = Convert.ToInt32(tag);

            if (Produto.Categoria > 0)
                LoadTipos(Produto.Categoria);
            else
                _ComboTipos.Clear(true);
        }



        private void SelectTipo_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            object tag = ((ComboBoxItem)select.SelectedItem).Tag;

            Produto.Tipo = Convert.ToInt32(tag);
        }



        private void SelectMarca_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectBox select = (SelectBox)sender;
            object tag = ((ComboBoxItem)select.SelectedItem).Tag;

            Produto.Marca = Convert.ToInt32(tag);
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
            Window owner = Application.Current.MainWindow;

            if (string.IsNullOrEmpty(Produto.Name))
                MessageBox.Show(owner, "Insira o nome do produto!", "Nome", MessageBoxButton.OK, MessageBoxImage.Stop);

            else if (Produto.Categoria <= 0)
                MessageBox.Show(owner, "Você precisa selecionar uma categoria!", "Categoria", MessageBoxButton.OK, MessageBoxImage.Stop);

            else if (Produto.Tipo <= 0)
                MessageBox.Show(owner, "Você precisa selecionar o tipo de produto!", "Tipo", MessageBoxButton.OK, MessageBoxImage.Stop);

            else if (Produto.Marca <= 0)
                MessageBox.Show(owner, "Você precisa selecionar a marca!", "Marca", MessageBoxButton.OK, MessageBoxImage.Stop);

            else if (Produto.Price <= 0)
                MessageBox.Show(owner, "O preço do produto precisa ser maior que 0 (zero)!", "Preço", MessageBoxButton.OK, MessageBoxImage.Stop);

            else
            {
                SalvarProduto();
            }
        }



        private void SalvarProduto()
        {
            int output = ProdutosViewModel.Create(
                    Produto.Name,
                    Produto.Categoria,
                    Produto.Tipo,
                    Produto.Marca,
                    Produto.Image,
                    Produto.Price,
                    Produto.Description
                );

            if (output > 0)
            {
                
            }
        }
    }
}
