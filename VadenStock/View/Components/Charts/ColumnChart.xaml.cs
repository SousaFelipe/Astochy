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
        public List<string> Columns = new();



        private double canvasWidth;
        private double canvasHeight;
        private double canvasMaxHeight;



        public ColumnChart()
        {
            InitializeComponent();

            Loaded += delegate
            {
                canvasWidth = _Canvas.ActualWidth;
                canvasHeight = _Canvas.ActualHeight;
            };
        }



        public void Series(double[] series)
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



        public void Labels(string[] labels)
        {
            if (labels.Length > 0)
            {
                foreach (string label in labels)
                    Columns.Add(label);
            }
        }



        public void Draw()
        {
            Loaded += delegate
            {
                Grid canvas = (Grid)_Canvas;

                for (int i = 0; i < Values.Count; i++)
                {
                    canvas.ColumnDefinitions.Add(new ColumnDefinition());

                    Line column = Column(Values[i], i);
                    canvas.Children.Add(column);

                    Grid.SetColumn(column, i);
                }
            };
        }



        public Line Column(double value, int pos)
        {
            //double x = (canvasWidth - (ColumnThickness * Values.Count)) / (Values.Count - 1);

            Line line = new();

            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            //line.X1 = (x * pos) + (ColumnThickness * pos) + (ColumnThickness / 2);
            //line.X2 = (x * pos) + (ColumnThickness * pos) + (ColumnThickness / 2);
            line.Y1 = ((value * 100) / canvasMaxHeight) * (canvasMaxHeight / 100);
            line.Y2 = canvasHeight;

            line.VerticalAlignment = VerticalAlignment.Bottom;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.StrokeStartLineCap = ColumnTopCornerStyle;
            line.StrokeEndLineCap = ColumnBottomCornerStyle;
            line.StrokeThickness = ColumnThickness;
            line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B0FF"));

            return line;
        }



        public TextBlock ColumnLabel(int pos)
        {
            TextBlock label = new();

            label.Text = Columns[pos];
            label.TextAlignment = TextAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Bottom;

            return label;
        }
    }
}
