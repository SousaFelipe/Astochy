using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Model.Types;



namespace VadenStock.View.Dialogs
{
    public partial class HistoricoDialog : Border
    {
        public AlmoxType Almoxarifado { get; private set; }
        public ItemType Item { get; private set; }



        public HistoricoDialog(AlmoxType almoxarifado, ItemType item)
        {
            Almoxarifado = almoxarifado;
            Item = item;

            InitializeComponent();

            Loaded += delegate
            {
                List<AlmoxTransfType> transferencias = 
            }
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
