using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Tools;



namespace VadenStock.View.Components.Charts
{
    public partial class BarChart : UserControl
    {
        private static readonly DependencyProperty RowLeftCornerStyleProp = DependencyProperty.Register(
                "RowLeftCornerStyle",
                typeof(PenLineCap),
                typeof(BarChart),
                new UIPropertyMetadata(PenLineCap.Flat, RowLeftCornerStyleCallback)
            );

        private static readonly DependencyProperty RowRightCornerStyleProp = DependencyProperty.Register(
                "RowRightCornerStyle",
                typeof(PenLineCap),
                typeof(BarChart),
                new UIPropertyMetadata(PenLineCap.Flat, RowRightCornerStyleCallback)
            );

        private static readonly DependencyProperty RowThicknessProp = DependencyProperty.Register(
                "RowThickness",
                typeof(double),
                typeof(BarChart),
                new UIPropertyMetadata(1.0, RowThicknessCallback)
            );

        private static readonly DependencyProperty LineColorProp = DependencyProperty.Register(
                "LineColor",
                typeof(string),
                typeof(BarChart),
                new UIPropertyMetadata("#00B0FF", LineColorCallback)
            );

        private static readonly DependencyProperty ShadowColorProp = DependencyProperty.Register(
                "ShadowColor",
                typeof(string),
                typeof(BarChart),
                new UIPropertyMetadata(string.Empty, ShadowColorCallback)
            );



        public PenLineCap RowLeftCornerStyle
        {
            get { return (PenLineCap)GetValue(RowLeftCornerStyleProp); }
            set { SetValue(RowLeftCornerStyleProp, value); }
        }

        public PenLineCap RowRightCornerStyle
        {
            get { return (PenLineCap)GetValue(RowRightCornerStyleProp); }
            set { SetValue(RowRightCornerStyleProp, value); }
        }

        public double RowThickness
        {
            get { return (double)GetValue(RowThicknessProp); }
            set { SetValue(RowThicknessProp, value); }
        }

        public string LineColor
        {
            get { return (string)GetValue(LineColorProp); }
            set { SetValue(LineColorProp, value); }
        }

        public string ShadowColor
        {
            get { return (string)GetValue(ShadowColorProp); }
            set { SetValue(ShadowColorProp, value); }
        }



        public static void RowLeftCornerStyleCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            BarChart chart = (BarChart)root;
            chart.RowLeftCornerStyle = (PenLineCap)e.NewValue;
        }

        public static void RowRightCornerStyleCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            BarChart chart = (BarChart)root;
            chart.RowRightCornerStyle = (PenLineCap)e.NewValue;
        }

        public static void RowThicknessCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            BarChart chart = (BarChart)root;
            chart.RowThickness = (double)e.NewValue;
        }

        public static void LineColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            BarChart chart = (BarChart)root;
            chart.LineColor = (string)e.NewValue;
        }

        public static void ShadowColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            BarChart chart = (BarChart)root;
            chart.ShadowColor = (string)e.NewValue;
        }



        public List<double> Values = new();
        public List<string> Labels = new();
        public List<Line> Lines = new();



        private double canvasWidth;
        private double canvasMaxWidth;
        private double canvasWidthLabels;



        public BarChart()
        {
            InitializeComponent();

            Loaded += delegate
            {
                canvasWidth = _Canvas.ActualWidth;
                canvasWidthLabels = 0.0;
            };
            
        }



        public void SetSeries(double[] series)
        {
            if (series.Length > 0)
            {
                int i;
                double currentMax = series[0];

                Values.Add(series[0]);

                for (i = 1; i < series.Length; i++)
                {
                    Values.Add(series[i]);

                    if (currentMax < series[i])
                        currentMax = series[i];
                }

                canvasMaxWidth = currentMax;
            }
        }



        public void SetLabels(string[] labels)
        {
            if (labels.Length > 0)
            {
                foreach (string label in labels)
                    Labels.Add(label);
            }
        }



        public void Draw(double externalPercent = 0.0, double bias = 0.0)
        {
            Loaded += delegate
            {
                double currentValue;
                double currentEndLine;

                if (Labels.Count >= Values.Count)
                {
                    DrawLabels();
                }

                for (int v = 0; v < Values.Count; v++)
                {
                    currentValue = Values[v];

                    if (currentValue > 0)
                        currentEndLine = externalPercent == 0.0
                            ? ((currentValue * (100 + bias)) / canvasMaxWidth) * (canvasMaxWidth / (100 + bias)) + canvasWidthLabels
                            : (canvasWidth * externalPercent) / (100 + bias);
                    else
                        currentEndLine = 1;

                    Line lineColumn = CreateLine(0.0, currentEndLine, LineColor);

                    _Canvas.RowDefinitions.Add(new RowDefinition());
                    _Canvas.Children.Add(lineColumn);
                     Canvas.SetZIndex(lineColumn, 2);

                    Grid.SetRow(lineColumn, v);

                    if (Labels.Count >= Values.Count)
                        Grid.SetColumn(lineColumn, 1);

                    Lines.Add(lineColumn);
                }

                if (ShadowColor != string.Empty)
                {
                    DrawShadows();
                }
            };
        }



        private void DrawShadows()
        {
            for (int s = 0; s < Lines.Count; s++)
            {
                Line lineRow = Lines[s];
                Line lineShadow = CreateLine(lineRow.X2, canvasWidth - RowThickness, ShadowColor);

                _Canvas.Children.Add(lineShadow);
                 Canvas.SetZIndex(lineShadow, 1);

                Grid.SetRow(lineShadow, s);

                if (Labels.Count >= Values.Count)
                    Grid.SetColumn(lineShadow, 1);
            }
        }



        private void DrawLabels()
        {
            ColumnDefinition col1 = new()
            {
                Width = GridLength.Auto
            };

            _Canvas.ColumnDefinitions.Add(col1);
            _Canvas.ColumnDefinitions.Add(new ColumnDefinition());

            for (int r = 0; r < Values.Count; r++)
            {
                TextBlock textLabel = TextLabel(r);

                _Canvas.Children.Add(textLabel);

                Grid.SetRow(textLabel, r);
                Grid.SetColumn(textLabel, 0);
            }

            canvasWidthLabels = 12;
        }



        public Line CreateLine(double startAt, double endAt, string hexColor)
        {
            Line line = new();

            line.SnapsToDevicePixels = true;
            line.X1 = startAt;
            line.X2 = endAt;
            line.Margin = new Thickness(0, 3, 0, 0);
            line.VerticalAlignment = VerticalAlignment.Center;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.StrokeStartLineCap = RowLeftCornerStyle;
            line.StrokeEndLineCap = RowRightCornerStyle;
            line.StrokeThickness = RowThickness;
            line.Stroke = Clr.Color(hexColor);

            return line;
        }



        public TextBlock TextLabel(int pos)
        {
            return new TextBlock()
            {
                Text = Labels[pos],
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }



        public void Clear()
        {
            Labels.Clear();
            Lines.Clear();
            _Canvas.Children.Clear();
        }
    }
}