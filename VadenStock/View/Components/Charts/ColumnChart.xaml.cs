using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;



namespace VadenStock.View.Components.Charts
{
    public partial class ColumnChart : UserControl
    {
        private static readonly DependencyProperty ColumnTopCornerStyleProp = DependencyProperty.Register(
                "ColumnTopCornerStyle",
                typeof(PenLineCap),
                typeof(ColumnChart),
                new UIPropertyMetadata(PenLineCap.Flat, ColumnTopCornerStyleCallback)
            );

        private static readonly DependencyProperty ColumnBottomCornerStyleProp = DependencyProperty.Register(
                "ColumnBottomCornerStyle",
                typeof(PenLineCap),
                typeof(ColumnChart),
                new UIPropertyMetadata(PenLineCap.Flat, ColumnBottomCornerStyleCallback)
            );

        private static readonly DependencyProperty ColumnThicknessProp = DependencyProperty.Register(
                "ColumnThickness",
                typeof(double),
                typeof(ColumnChart),
                new UIPropertyMetadata(1.0, ColumnThicknessCallback)
            );

        private static readonly DependencyProperty LineColorProp = DependencyProperty.Register(
                "LineColor",
                typeof(string),
                typeof(ColumnChart),
                new UIPropertyMetadata("#00B0FF", LineColorCallback)
            );

        private static readonly DependencyProperty ShadowColorProp = DependencyProperty.Register(
                "ShadowColor",
                typeof(string),
                typeof(ColumnChart),
                new UIPropertyMetadata(string.Empty, ShadowColorCallback)
            );



        public PenLineCap ColumnTopCornerStyle
        {
            get { return (PenLineCap)GetValue(ColumnTopCornerStyleProp); }
            set { SetValue(ColumnTopCornerStyleProp, value); }
        }

        public PenLineCap ColumnBottomCornerStyle
        {
            get { return (PenLineCap)GetValue(ColumnBottomCornerStyleProp); }
            set { SetValue(ColumnBottomCornerStyleProp, value); }
        }

        public double ColumnThickness
        {
            get { return (double)GetValue(ColumnThicknessProp); }
            set { SetValue(ColumnThicknessProp, value); }
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



        public static void ColumnTopCornerStyleCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ColumnChart chart = (ColumnChart)root;
            chart.ColumnTopCornerStyle = (PenLineCap)e.NewValue;
        }

        public static void ColumnBottomCornerStyleCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ColumnChart chart = (ColumnChart)root;
            chart.ColumnBottomCornerStyle = (PenLineCap)e.NewValue;
        }

        public static void ColumnThicknessCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ColumnChart chart = (ColumnChart)root;
            chart.ColumnThickness = (double)e.NewValue;
        }

        public static void LineColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ColumnChart chart = (ColumnChart)root;
            chart.LineColor = (string)e.NewValue;
        }

        public static void ShadowColorCallback(DependencyObject root, DependencyPropertyChangedEventArgs e)
        {
            ColumnChart chart = (ColumnChart)root;
            chart.ShadowColor = (string)e.NewValue;
        }



        public List<double> Values = new();
        public List<string> Labels = new();
        public List<Line> Lines = new();



        private double canvasHeight;
        private double canvasMaxHeight;
        private double canvasHeightLabels;



        public ColumnChart()
        {
            InitializeComponent();

            Loaded += delegate
            {
                canvasHeight = _Canvas.ActualHeight;
                canvasHeightLabels = 0.0;
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

                canvasMaxHeight = currentMax;
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



        public void Draw(double bias = 0.0)
        {
            Loaded += delegate
            {
                double currentValue;
                double currentStartLine;

                if (Labels.Count >= Values.Count)
                {
                    DrawLabels();
                }

                for (int v = 0; v < Values.Count; v++)
                {
                    currentValue = Values[v];

                    if (currentValue > 0)
                    {
                        currentStartLine = ((currentValue * 100) / canvasMaxHeight) * (canvasMaxHeight / 100) + canvasHeightLabels;

                        Line lineColumn = CreateLine(currentStartLine, canvasHeight, LineColor);

                        _Canvas.ColumnDefinitions.Add(new ColumnDefinition());
                        _Canvas.Children.Add(lineColumn);
                         Canvas.SetZIndex(lineColumn, 1);

                        Grid.SetRow(lineColumn, 0);
                        Grid.SetColumn(lineColumn, v);

                        Lines.Add(lineColumn);
                    }
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
                Line lineColumn = Lines[s];
                Line lineShadow = CreateLine(
                        0.0 + (ColumnThickness * 2.5),
                        lineColumn.Y2,
                        ShadowColor
                    );

                _Canvas.Children.Add(lineShadow);
                Canvas.SetZIndex(lineShadow, 0);

                Grid.SetColumn(lineShadow, s);
                Grid.SetRow(lineShadow, 0);
            }
        }



        private void DrawLabels()
        {
            RowDefinition row1 = new()
            {
                Height = GridLength.Auto
            };

            _Canvas.RowDefinitions.Add(new RowDefinition());
            _Canvas.RowDefinitions.Add(row1);

            for (int r = 0; r < Values.Count; r++)
            {
                TextBlock textLabel = TextLabel(r);

                _Canvas.Children.Add(textLabel);

                Grid.SetRow(textLabel, 1);
                Grid.SetColumn(textLabel, r);
            }

            canvasHeightLabels = 12;
        }



        public Line CreateLine(double startLine, double endLine, string hexColor)
        {
            Line line = new();

            line.SnapsToDevicePixels = true;
            line.Y1 = startLine;
            line.Y2 = endLine;
            line.Margin = new Thickness(4, 0, 0, 0);
            line.VerticalAlignment = VerticalAlignment.Bottom;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.StrokeStartLineCap = ColumnTopCornerStyle;
            line.StrokeEndLineCap = ColumnBottomCornerStyle;
            line.StrokeThickness = ColumnThickness;
            line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));

            return line;
        }



        public TextBlock TextLabel(int pos)
        {
            return new TextBlock()
            {
                Text = Labels[pos],
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
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
