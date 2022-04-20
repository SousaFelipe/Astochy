using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Diagnostics;

namespace VadenStock.View.Components.Charts
{
    public partial class BarChart : UserControl
    {
        private double[]? Values;
        private string[]? Labels;


        private double maxFromValues = 0;



        public string BarsBackColor { get; set; }
        public string BarsLineColor { get; set; }
        public double BarsThickness { get; set; }
        public double BarsCorners { get; set; }
        public Orientation ChartDirection { get; set; }



        public BarChart()
        {
            BarsBackColor = "#CCCCCC";
            BarsLineColor = "#1234EF";
            ChartDirection = Orientation.Horizontal;

            BarsThickness = 6;
            BarsCorners = 3;

            InitializeComponent();
        }



        public BarChart(Orientation orientation = Orientation.Horizontal)
        {
            BarsBackColor = "#CCCCCC";
            BarsLineColor = "#1234EF";
            ChartDirection = Orientation.Horizontal;

            BarsThickness = 6;
            BarsCorners = 3;

            InitializeComponent();
        }



        public void SetDataset(double[] dataset)
        {
            Values = dataset;

            if (Values != null && Values.Length > 0)
            {
                maxFromValues = dataset[0];

                for (int i = 1; i < Values.Length; i++)
                {
                    if (Values[i] > maxFromValues)
                        maxFromValues = Values[i];
                }
            }
        }



        public void SetLabels(string[] labels)
        {
            Labels = labels;
        }



        public void DrawBars()
        {
            if (Values != null && Values.Length > 0)
            {
                _StackContainer.Children.Clear();

                double currentValue;
                bool orientationIsH;

                for (int i = 0; i < Values.Length; i++)
                {
                    currentValue = Values[i];
                    orientationIsH = (ChartDirection == Orientation.Horizontal);

                    Bar bar = new(orientationIsH ? Orientation.Vertical : Orientation.Horizontal)
                    {
                        BackColor = BarsBackColor,
                        BarColor = BarsLineColor,
                        BorderRadius = BarsCorners,

                        VerticalAlignment = orientationIsH
                            ? VerticalAlignment.Stretch
                            : VerticalAlignment.Center,

                        HorizontalAlignment = orientationIsH
                            ? HorizontalAlignment.Left
                            : HorizontalAlignment.Stretch
                    };

                    _StackContainer.Children.Add(bar);

                    if (orientationIsH)
                        bar.Width = BarsThickness;
                    else
                        bar.Height = BarsThickness;

                    bar.UpdateBar((currentValue * 100) / maxFromValues);
                }
            }
        }
    }
}
