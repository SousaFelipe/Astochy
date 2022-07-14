using System;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.View.Models;
using VadenStock.View.Components.Widgets;

using VadenStock.Model.Types;



namespace VadenStock.View.Dialogs
{
    public partial class HistoricoDialog : Border
    {
        public AlmoxType Almoxarifado { get; private set; }
        public ItemType Item { get; private set; }
        public object? View { get; private set; }



        public HistoricoDialog(AlmoxType almoxarifado, ItemType item, object parent)
        {
            Almoxarifado = almoxarifado;
            Item = item;
            View = parent;

            InitializeComponent();

            Loaded += delegate
            {
                LoadDetails();
                LoadTransferencias();
            };
        }



        private void LoadDetails()
        {
            _TextEntradaData.Text = Item.CreatedDate.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ");
            _TextAlmoxRespons.Text = Item.Almoxarifado.Name;
        }



        private void LoadTransferencias()
        {
            List<AlmoxTransfType> transferencias = TransferenciasViewModel.TransfsPorItem(Item.Id);

            if (transferencias.Count > 0)
            {
                AlmoxTransfType current;

                for (int t = 0; t < transferencias.Count; t++)
                {
                    current = transferencias[t];

                    if (Almoxarifado.Id == current.To.Id)
                        _StackHistoryBlocks.Children.Add(new HistoryInBlock(current));
                    
                    else
                        _StackHistoryBlocks.Children.Add(new HistoryOutBlock(current));
                }
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MethodInfo? method = View?.GetType().GetMethod("BackToMain");
            method?.Invoke(View, new object?[] { this });
        }



        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.CloseDialog(this);
        }
    }
}
