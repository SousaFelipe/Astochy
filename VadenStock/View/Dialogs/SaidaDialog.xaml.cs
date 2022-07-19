using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;

using VadenStock.Model.Types;

using VadenStock.Core.Http;

using VadenStock.View.Models;
using VadenStock.View.Components.Forms;
using VadenStock.View.Components.Containers;

using VadenStock.Http;

using VadenStock.Tools;
using Newtonsoft.Json.Linq;

namespace VadenStock.View.Dialogs
{
    public partial class SaidaDialog : Border
    {
        public SaidaDialog()
        {
            InitializeComponent();

            Loaded += delegate
            {
                _InputCliente.OnSearch((result) =>
                {
                    System.Diagnostics.Trace.WriteLine(result);
                });
            };
        }



        private void InputCodigo_Changed(object sender, TextChangedEventArgs e)
        {

        }



        private void SelectAcoes_Changed(object sender, SelectionChangedEventArgs e)
        {

        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
