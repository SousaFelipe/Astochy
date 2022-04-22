﻿using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.View.Models;
using VadenStock.View.Structs;
using VadenStock.View.Components;

using VadenStock.Model;
using System.Windows.Media;



namespace VadenStock.View
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();

            Loaded += delegate {
                LoadCardPatrimonio();
                LoadCardEstoqueMinMax();
                LoadAlmoxCards();
            };
        }



        public void LoadCardPatrimonio()
        {
            PatrimonioS patrimonio = DashboardViewModel.GetPatrimonio();

            _PatrimonioEmEstoque.UpdateValue(patrimonio.Estoque, patrimonio.Total);
            _PatrimonioEmComodato.UpdateValue(patrimonio.Comodato, patrimonio.Total);
            _PatrimonioEmProducao.UpdateValue(patrimonio.Producao, patrimonio.Total);
        }



        private void LoadCardEstoqueMinMax()
        {
            var categorias = DashboardViewModel.Categorias;

            for (int i = 0; i < categorias.Count; i++)
            {
                _ComboCategorias.Items.Add(new ComboBoxItem()
                {
                    Tag = categorias[i].Id,
                    Content = string.Concat(categorias[i].Name[0], categorias[i].Name[1..].ToLower())
                });
            }

            _ColumnChartMinMax.SetSeries(new double[] { 45, 23, 84, 19, 61, 42, 77 });
            _ColumnChartMinMax.SetLabels(new string[] { "MKT", "UBQ", "ITB", "HWY", "SAE", "OPB", "LNA" });
            _ColumnChartMinMax.Draw();
        }



        private void LoadAlmoxCards()
        {
            List<Almoxarifado.Contract> almoxarifados = DashboardViewModel.GetAlmoxarifados();

            if (almoxarifados != null)
            {
                Almoxarifado.Contract currentContract;
                AlmoxCardDash currentCard;

                int rounds = 0;
                int crrRow = 0;

                for (int a = 0; a < almoxarifados.Count; a++)
                {
                    currentContract = almoxarifados[a];

                    currentCard = new AlmoxCardDash(currentContract)
                    {
                        Margin = new Thickness(6, 0, 6, 0),
                        CardColor = "#FFFFFF",
                        Tipo = currentContract.Tipo
                    };

                    currentCard.SetItens(DashboardViewModel.GetItems(currentContract.Id));

                    _GridAlmoxarifados.Children.Add(currentCard);
                     Grid.SetColumn(currentCard, rounds);
                     Grid.SetRow(currentCard, crrRow);

                    if (0 == (rounds - 3))
                    {
                        rounds = 0;
                        crrRow++;

                        _GridAlmoxarifados.RowDefinitions.Add(new RowDefinition());
                    }
                    else
                    {
                        rounds++;
                    }
                }
            }
        }



        private void ComboCategorias_Update(object sender, SelectionChangedEventArgs e)
        {
            object tag = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            int tagNum = int.Parse(tag.ToString());

            if (_ComboTipos != null)
            {
                var tipos = DashboardViewModel.GetTipos(tagNum);

                _ComboTipos.Clear(true);

                for (int i = 0; i < tipos.Count; i++)
                {
                    _ComboTipos.Items.Add(new ComboBoxItem()
                    {
                        Tag = tipos[i].Id,
                        Content = string.Concat(tipos[i].Name[0], tipos[i].Name[1..].ToLower())
                    });
                }
            }
        }
    }
}
