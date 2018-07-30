using PersonalShopper.Db;
using PersonalShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PersonalShopper.ViewModel
{
    class BarGraphModel
    {
        public BarGraphModel(IDbOperations repository)
        {
            Repository = repository;
            BarGraphCanvas = new Canvas { Background = Brushes.White, MinWidth = 560, MinHeight = 190 };
            CreateBarGraph();
        }        
        public IDbOperations Repository { get; private set; }

        public Canvas BarGraphCanvas { get; private set; }

        private void CreateBarGraph()
        {
            var barGraphData = new MonthlyChart(Repository).GenerateBarGraphData();

            var label = new Label { Content = "Last 12 Month Expenses", FontSize = 22 };
            BarGraphCanvas.Children.Add(label);
            Canvas.SetLeft(label, 200);
            Canvas.SetTop(label, 10);

            AddBarsToView(barGraphData);
        }
        private void AddBarsToView(List<MonthlyExpense> barGraphData)
        {
            double init = 40;
            Label label, xAxisLabel;

            var maxExp = barGraphData.Select(x => x.MonthlyExpenditure).Max();

            foreach (var month in barGraphData)
            {
                #region BarConfiguration
                System.Windows.Shapes.Rectangle bar = new System.Windows.Shapes.Rectangle
                {
                    Width = 25,
                    Height = ((double)(month.MonthlyExpenditure / maxExp)) * 125,
                    Margin = new Thickness(10, 5, 10, 5),
                    Fill = Brushes.MediumVioletRed,
                };

                Canvas.SetLeft(bar, init);
                Canvas.SetBottom(bar, 25);
                #endregion

                #region BarLabel Configuration
                label = new Label
                { Content = Math.Round(month.MonthlyExpenditure, 2) };

                Canvas.SetLeft(label, init-5);
                var labelHeight = 25 + bar.Height + 5; //bottom heigh + bar height + space betweeen bar and label
                Canvas.SetBottom(label, labelHeight);
                #endregion

                #region X-Axis Label Configuration

                xAxisLabel = new Label { Content = $"{month.Month}" };

                Canvas.SetLeft(xAxisLabel, init);
                var xAxisLabelHeight = 5;
                Canvas.SetBottom(xAxisLabel, xAxisLabelHeight);

                #endregion

                #region Add To Canvas
                BarGraphCanvas.Children.Add(bar);
                BarGraphCanvas.Children.Add(label);
                BarGraphCanvas.Children.Add(xAxisLabel);

                #endregion

                init += 50;
            }

            var xAxis = new Line
            { Stroke = Brushes.Black, StrokeThickness = 1 };
            xAxis.X1 = 1; xAxis.X2 = init - 50;
            BarGraphCanvas.Children.Add(xAxis);
            Canvas.SetLeft(xAxis, 30);
            Canvas.SetBottom(xAxis, 30);

            var yAxis = new Line { Stroke = Brushes.Black, StrokeThickness = 1 };
            yAxis.Y1 = 1; yAxis.Y2 = 145;
            BarGraphCanvas.Children.Add(yAxis);
            Canvas.SetLeft(yAxis, 30);
            Canvas.SetBottom(yAxis, 30);
        }
    }
}





