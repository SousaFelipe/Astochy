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
        private static readonly DependencyProperty BarsBackColorProp = DependencyProperty.Register(
                "BarsBackColor",
                typeof(string),
                typeof(BarChart),
                new UIPropertyMetadata("#ECEFF1")
            );

        private static readonly DependencyProperty BarsLineColorProp = DependencyProperty.Register(
                "BarsLineColor",
                typeof(string),
                typeof(BarChart),
                new UIPropertyMetadata("#00B0FF")
            );

        private static readonly DependencyProperty BarsThicknessProp = DependencyProperty.Register(
                "BarsThickness",
                typeof(double),
                typeof(BarChart),
                new UIPropertyMetadata(6.0)
            );

        private static readonly DependencyProperty BarsCornersProp = DependencyProperty.Register(
                "BarsCorners",
                typeof(double),
                typeof(BarChart),
                new UIPropertyMetadata(0.0)
            );

        private static readonly DependencyProperty ChartDirectionProp = DependencyProperty.Register(
                "ChartDirection",
                typeof(Orientation),
                typeof(BarChart),
                new UIPropertyMetadata(Orientation.Horizontal)
            );



        public string BarsBackColor
        {
            get { return (string)GetValue(BarsBackColorProp); }
            set { SetValue(BarsBackColorProp, value); }
        }

        public string BarsLineColor
        {
            get { return (string)GetValue(BarsLineColorProp); }
            set { SetValue(BarsLineColorProp, value); }
        }

        public double BarsThickness
        {
            get { return (double)GetValue(BarsThicknessProp); }
            set { SetValue(BarsThicknessProp, value); }
        }

        public double BarsCorners
        {
            get { return (double)GetValue(BarsCornersProp); }
            set { SetValue(BarsCornersProp, value); }
        }

        public Orientation ChartDirection
        {
            get { return (Orientation)GetValue(ChartDirectionProp); }
            set { SetValue(ChartDirectionProp, value); }
        }



        private double[]? Values;
        private string[]? Labels;


        private double maxFromValues = 0;



        public BarChart()
        {
            InitializeComponent();
        }



        public BarChart(Orientation orientation = Orientation.Horizontal)
        {
            ChartDirection = orientation;
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
                bool orientationIsH = (ChartDirection == Orientation.Horizontal);
                double currentValue;

                _StackContainer.Children.Clear();

                for (int i = 0; i < Values.Length; i++)
                {
                    currentValue = Values[i];

                    Bar bar = new(orientationIsH ? Orientation.Vertical : Orientation.Horizontal)
                    {
                        BackColor = BarsBackColor,
                        BarColor = BarsLineColor,
                        BorderRadius = BarsCorners
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
