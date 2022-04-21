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



        public List<double> Values = new();
        public List<string> Labels = new();



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



        public void Draw()
        {
            Loaded += delegate
            {
                if (Labels.Count >= Values.Count)
                {
                    DrawLabels();
                }

                for (int v = 0; v < Values.Count; v++)
                {
                    Line lineColumn = LineColumn(v);

                    _Canvas.ColumnDefinitions.Add(new ColumnDefinition());
                    _Canvas.Children.Add(lineColumn);

                    Grid.SetRow(lineColumn, 0);
                    Grid.SetColumn(lineColumn, v);
                }
            };
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



        public Line LineColumn(int pos)
        {
            Line line = new();

            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            line.Y1 = ((Values[pos] * 100) / canvasMaxHeight) * (canvasMaxHeight / 100) + canvasHeightLabels;
            line.Y2 = canvasHeight;

            line.VerticalAlignment = VerticalAlignment.Bottom;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.StrokeStartLineCap = ColumnTopCornerStyle;
            line.StrokeEndLineCap = ColumnBottomCornerStyle;
            line.StrokeThickness = ColumnThickness;
            line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B0FF"));

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
    }
}
