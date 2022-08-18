using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

                if (!string.IsNullOrEmpty(item.Mac))
                {
                    _TextMAC.Text = Str.MAC(item.Mac);
                    _TextMAC.Cursor = Cursors.Hand;
                    _TextMAC.MouseEnter += Text_MouseEnter;
                    _TextMAC.MouseLeave += Text_MouseLeave;
                    _TextMAC.MouseLeftButtonUp += TextMAC_MouseLeftButtonUp;
                }

                _TextAlmox.Text = item.Almoxarifado.Name;

                _TextUltTransf.Text = (item.UltimaTransf != null)
                    ? item.UltimaTransf.Value.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ")
                    : "__/__/____";
            };
        }



        private void Text_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock text = (TextBlock)sender;

            text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0094CC"));
            text.TextDecorations = TextDecorations.Underline;
        }



        private void Text_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock text = (TextBlock)sender;

            text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#607D8B"));
            text.TextDecorations = null;
        }



        private void TextCodigo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new HistoricoDialog(item));
        }



        private void TextMAC_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new HistoricoDialog(item));
        }



        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.DisplayDialog(new HistoricoDialog(item));
        }
    }
}
