using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

using VadenStock.Tools;



namespace VadenStock.View.Components.Cards
{
    public partial class ThumbnailCard : Border
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register(
                "Body",
                typeof(string),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(string.Empty, BodyPropertyCallback)
            );

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
                "Header",
                typeof(string),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(string.Empty, HeaderPropertyCallback)
            );

        public static readonly DependencyProperty HeaderSizeProperty = DependencyProperty.Register(
                "HeaderSize",
                typeof(int),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(13, HeaderSizePropertyCallback)
            );

        public static readonly DependencyProperty SubHeaderProperty = DependencyProperty.Register(
                "SubHeader",
                typeof(string),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(string.Empty, SubHeaderPropertyCallback)
            );

        public static readonly DependencyProperty SubHeaderSizeProperty = DependencyProperty.Register(
                "SubHeaderSize",
                typeof(int),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(10, SubHeaderSizePropertyCallback)
            );



        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

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



        public static void BodyPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            string body = (string)e.NewValue;
            thumb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(body));
        }

        public static void HeaderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            thumb._TextHeader.Text = (string)e.NewValue;
        }

        public static void HeaderSizePropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            thumb._TextHeader.FontSize = (int)e.NewValue;
        }

        public static void SubHeaderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            thumb._TextSubHeader.Text = (string)e.NewValue;
        }

        public static void SubHeaderSizePropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            thumb._TextSubHeader.FontSize = (int)e.NewValue;
        }



        public ThumbnailCard()
        {
            InitializeComponent();
        }



        public void SetThumb(string fileName)
        {
            _ImageThumb.Source = Src.Icon(fileName);
            _ImageThumb.UpdateLayout();
        }
    }
}
