using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



namespace VadenStock.View.Components.Forms
{
    public partial class InputSearch : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
                "Placeholder",
                typeof(string),
                typeof(InputSearch),
                new UIPropertyMetadata(string.Empty, PlaceholderPropertyCallback)
            );



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }



        public static void PlaceholderPropertyCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            InputText instance = (InputText)root;
            instance.Placeholder = (string)e.NewValue;
        }



        public UIElement? Popup { get; private set; }
        public StackPanel? Container { get; private set; }



        private bool CanDecay;
        private int SearchDecay;
        private Action<string>? Callback;



        public InputSearch()
        {
            InitializeComponent();

            SearchDecay = 100;
            CanDecay = false;

            Loaded += delegate
            {
                TextChanged += InputText_Changed;

                BootObserver();
            };
        }



        private void BootObserver()
        {
            Application.Current.Dispatcher.Invoke(async () => {
                for(;;)
                {
                    await Task.Delay(25);

                    if (CanDecay)
                    {
                        if (SearchDecay > 0)
                            SearchDecay -= 1;
                        else
                        {
                            Callback?.Invoke(Text);
                            CanDecay = false;
                            SearchDecay = 50;
                        }
                    }
                }
            });
        }



        public void SetPopup(UIElement popup)
        {
            Popup = popup;
        }



        public void SetContainer(StackPanel container)
        {
            Container = container;
        }



        public void OnSearch(Action<string> callback)
        {
            Callback = callback;
        }



        public void Select(string text)
        {
            Callback = null;
            SearchDecay = 50;
            CanDecay = false;

            if (Container != null)
                Container.Children.Clear();

            if (Popup != null)
                Popup.Visibility = Visibility.Collapsed;

            Text = text;
            CaretIndex = Text.Length;
        }



        private void InputText_Changed(object sender, TextChangedEventArgs e)
        {
            SearchDecay = 50;
            
            if (!CanDecay)
                CanDecay = true;
        }
    }
}
