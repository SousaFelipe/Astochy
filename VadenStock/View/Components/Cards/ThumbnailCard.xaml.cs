using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;



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

        public static readonly DependencyProperty SubHeaderProperty = DependencyProperty.Register(
                "SubHeader",
                typeof(string),
                typeof(ThumbnailCard),
                new UIPropertyMetadata(string.Empty, SubHeaderPropertyCallback)
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
        
        public string SubHeader
        {
            get { return (string)GetValue(SubHeaderProperty); }
            set { SetValue(SubHeaderProperty, value); }
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

        public static void SubHeaderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailCard thumb = (ThumbnailCard)root;
            thumb._TextSubHeader.Text = (string)e.NewValue;
        }



        public ThumbnailCard()
        {
            InitializeComponent();
        }



        public void SetThumb(string name)
        {
            _ImageThumb.Source = Utils.FindResource(name, Utils.Resource.Icons, Utils.Resource.FileType.PNG);
            _ImageThumb.UpdateLayout();
        }
    }
}
