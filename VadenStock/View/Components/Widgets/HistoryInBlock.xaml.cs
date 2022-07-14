using System;
using System.Windows;
using System.Windows.Controls;

using VadenStock.Model.Types;

using VadenStock.Tools;



namespace VadenStock.View.Components.Widgets
{
    public partial class HistoryInBlock : Grid
    {
        public AlmoxTransfType Transferencia { get; private set; }



        public HistoryInBlock(AlmoxTransfType transferencia)
        {
            Transferencia = transferencia;

            InitializeComponent();

            Loaded += delegate
            {
                _ImageIcon.Source = Src.Icon(transferencia.To.GetIcon("black"));
                _TextTransfData.Text = Transferencia.CreatedDate.ToString("dd/MM/yyyy HH:mm").Replace(" ", " às ");
                _TextAlmox.Text = Transferencia.To.Name;
                _TextDescription.Text = Transferencia.Description;
            };
        }
    }
}
