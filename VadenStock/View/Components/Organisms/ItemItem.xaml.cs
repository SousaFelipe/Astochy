using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;

using VadenStock.Tools;
using VadenStock.View.Dialogs;



namespace VadenStock.View.Components.Organisms
{
    public partial class ItemItem : Grid
    {
        private readonly ItemType item;



        public ItemItem()
        {
            InitializeComponent();
        }


        
        public ItemItem(ItemType item)
        {
            this.item = item;

            InitializeComponent();

            Loaded += delegate
            {
                _TextCodigo.Text = item.Codigo.ToString();
                _TextProduto.Text = item.Produto.Name;
                _TextMAC.Text = Str.MAC(item.Mac);
                _TextAlmox.Text = item.Almoxarifado.Name;
                _TextUltTransf.Text = item.UltimaTransf.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ");
            };
        }



        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new HistoricoDialog(item));
        }
    }
}
