using System;
using System.Threading.Tasks;
using System.Timers;
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



        private Task? SearchTask;
        private Action<string>? Callback;



        public InputSearch()
        {
            InitializeComponent();

            Loaded += delegate
            {
                TextChanged += InputText_Changed;
            };
        }



        public void OnSearch(Action<string> callback)
        {
            Callback = callback;
        }



        private void InputText_Changed(object sender, TextChangedEventArgs e)
        {
            InputSearch input = (InputSearch)sender;
            string text = input.Text;

            SearchTask?.Dispose();
            SearchTask = null;

            Application.Current.Dispatcher.Invoke(() => {
                SearchTask = new Task(async () =>
                {
                    await Task.Delay(1000);
                    Callback?.Invoke(text);
                });
            });

            SearchTask?.Start();
        }
    }
}
