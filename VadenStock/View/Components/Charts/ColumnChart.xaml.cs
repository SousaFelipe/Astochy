using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;

using VadenStock.Tools;
using System.Globalization;

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
        public List<string> Tags = new();
        public List<Line> Lines = new();



        private double canvasMaxHeight;
        private double canvasHeightLabels;



        public ColumnChart()
        {
            InitializeComponent();
        }



        public void SetSeries(double[] series)
        {
            if (series != null && series.Length > 0)
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
            if (labels != null && labels.Length > 0)
            {
                foreach (string label in labels)
                    Labels.Add(label);
            }
        }



        public void SetTags(string[] tags)
        {
            if (tags != null && tags.Length > 0)
            {
                foreach (string tag in tags)
                    Tags.Add(tag);
            }
        }



        public void Draw()
        {
            #region Draw labels
            if (Labels.Count >= Values.Count)
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
            #endregion


            #region Draw lines
            for (int v = 0; v < Values.Count; v++)
            {
                Line lineColumn = CreateLine(StartLineFromCanvasHeight(Values[v]), _Canvas.ActualHeight, LineColor);
                TextBlock block = TextTag(lineColumn.Y1, v);

                _Canvas.ColumnDefinitions.Add(new ColumnDefinition());
                _Canvas.Children.Add(lineColumn);
                _Canvas.Children.Add(block);

                Grid.SetRow(lineColumn, 0);
                Grid.SetColumn(lineColumn, v);
                Panel.SetZIndex(lineColumn, 0);

                Grid.SetRow(block, 0);
                Grid.SetColumn(block, v);
                Panel.SetZIndex(block, 1);

                Lines.Add(lineColumn);
            }
            #endregion


            #region Draw shadows
            if (ShadowColor != string.Empty)
            {
                for (int s = 0; s < Lines.Count; s++)
                {
                    Line lineColumn = Lines[s];
                    Line lineShadow = CreateLine(
                            ColumnThickness * 2.5,
                            lineColumn.Y2,
                            ShadowColor
                        );

                    _Canvas.Children.Add(lineShadow);
                    Canvas.SetZIndex(lineShadow, 0);

                    Grid.SetColumn(lineShadow, s);
                    Grid.SetRow(lineShadow, 0);
                }
            }
            #endregion
        }



        private Line CreateLine(double startLine, double endLine, string hexColor)
        {
            return new Line()
            {
                SnapsToDevicePixels = true,
                Y1 = startLine,
                Y2 = endLine,
                Margin = new Thickness(4, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                StrokeStartLineCap = ColumnTopCornerStyle,
                StrokeEndLineCap = ColumnBottomCornerStyle,
                StrokeThickness = ColumnThickness,
                Stroke = Clr.Color(hexColor)
            };
        }



        private TextBlock TextTag(double y, int pos)
        {
            double top = (y - 38);

            TextBlock block = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, top, 0, 0),
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 09,
                Text = Tags.Count > pos ? Tags[pos] : string.Empty,
            };

            return block;
        }



        private TextBlock TextLabel(int pos)
        {
            return new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontSize = 9,
                Text = Labels[pos].ToUpper(),
            };
        }



        private double StartLineFromCanvasHeight(double value)
        {
            double canvasHeight = _Canvas.ActualHeight;
            double startLine = canvasHeight;

            if (value > 0)
            {
                double percent = Clc.PercentFromVal(value, canvasMaxHeight);
                startLine = (canvasHeight - Clc.ValFromPercent(percent, canvasHeight)) + canvasHeightLabels + (ColumnThickness * 1.5);
            }

            return startLine;
        }



        public void Clear()
        {
            Values.Clear();
            Labels.Clear();
            Lines.Clear();

            _Canvas.Children.Clear();
            _Canvas.RowDefinitions.Clear();
            _Canvas.ColumnDefinitions.Clear();
        }
    }
}
