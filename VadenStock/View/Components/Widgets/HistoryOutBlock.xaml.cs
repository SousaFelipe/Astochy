using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;



namespace VadenStock.View.Components.Widgets
{
    public partial class HistoryOutBlock : Grid
    {
        public AlmoxTransfType Transferencia { get; private set; }



        public HistoryOutBlock(AlmoxTransfType transferencia)
        {
            Transferencia = transferencia;

            InitializeComponent();

            Loaded += delegate
            {
                _TextTransfData.Text = Transferencia.CreatedDate.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ");
                _TextAlmox.Text = Transferencia.To.Name;
                _TextAlmox.Text = Transferencia.Description;
            };
        }
    }
}
