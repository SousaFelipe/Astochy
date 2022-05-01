using System.Windows;
using System.Windows.Controls;

using VadenStock.Tools;



namespace VadenStock.View.Components.Cards
{
    public partial class MidiaThumbCard : Border
    {

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
                "Header",
                typeof(string),
                typeof(MidiaThumbCard),
                new UIPropertyMetadata(string.Empty, HeaderPropertyCallback)
            );

        public static readonly DependencyProperty HeaderSizeProperty = DependencyProperty.Register(
                "HeaderSize",
                typeof(int),
                typeof(MidiaThumbCard),
                new UIPropertyMetadata(13, HeaderSizePropertyCallback)
            );

        public static readonly DependencyProperty SubHeaderProperty = DependencyProperty.Register(
                "SubHeader",
                typeof(string),
                typeof(MidiaThumbCard),
                new UIPropertyMetadata(string.Empty, SubHeaderPropertyCallback)
            );

        public static readonly DependencyProperty SubHeaderSizeProperty = DependencyProperty.Register(
                "SubHeaderSize",
                typeof(int),
                typeof(MidiaThumbCard),
                new UIPropertyMetadata(10, SubHeaderSizePropertyCallback)
            );



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public int HeaderSize
        {
            get { return (int)GetValue(HeaderSizeProperty); }
            set { SetValue(HeaderSizeProperty, value); }
        }

        public string SubHeader
        {
            get { return (string)GetValue(SubHeaderProperty); }
            set { SetValue(SubHeaderProperty, value); }
        }

        public int SubHeaderSize
        {
            get { return (int)GetValue(SubHeaderSizeProperty); }
            set { SetValue(SubHeaderSizeProperty, value); }
        }



        public static void HeaderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            MidiaThumbCard midia = (MidiaThumbCard)root;
            midia._TextHeader.Text = (string)e.NewValue;
        }

        public static void HeaderSizePropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            MidiaThumbCard midia = (MidiaThumbCard)root;
            midia._TextHeader.FontSize = (int)e.NewValue;
        }

        public static void SubHeaderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            MidiaThumbCard midia = (MidiaThumbCard)root;
            midia._TextSubHeader.Text = (string)e.NewValue;
        }

        public static void SubHeaderSizePropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            MidiaThumbCard midia = (MidiaThumbCard)root;
            midia._TextSubHeader.FontSize = (int)e.NewValue;
        }



        public MidiaThumbCard()
        {
            InitializeComponent();
        }



        public void SetMidia(string fileName)
        {
            _ImageThumb.Source = Src.Storage(fileName);
            _ImageThumb.UpdateLayout();
        }
    }
}
