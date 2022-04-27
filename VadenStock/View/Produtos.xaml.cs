using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using VadenStock.View.Models;
using VadenStock.View.Adapters;
using VadenStock.View.Dialogs;



namespace VadenStock.View
{
    public partial class Produtos : UserControl
    {
        public Produtos()
        {
            InitializeComponent();

            Loaded += delegate
            {
                LoadProdutos();
            };
        }



        public void LoadProdutos()
        {
            ProdutosAdapter adapter = new(_GridProdutos);

            adapter.Update(ProdutosViewModel.GetProdutos(), false);
            adapter.Build();
        }



        public void OpenNovoProdutoDialog()
        {
            VadenStock.MainWindow window = (VadenStock.MainWindow)Application.Current.MainWindow;
            window.EnterDialogMode();

            ProdutoDialog dialog = new();
            dialog.ShowDialog();
        }



        private void ComboCadastroSelectedOption(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedItem is ComboBoxItem cbi)
            {
                switch (cbi.Tag)
                {
                    case "P":
                        OpenNovoProdutoDialog();
                        break;

                    case "M":
                        break;

                    case "C":
                        break;

                    case "T":
                        break;
                }

                cb.SelectedItem = null;
            }
        }
    }
}
