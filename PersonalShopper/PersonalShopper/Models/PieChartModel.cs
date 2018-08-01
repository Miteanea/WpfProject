using PersonalShopper.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace PersonalShopper.Models
{
    public class PieChartModel
    {
        public PieChartModel(IDbOperations repository)
        {
            PieGraph = new Grid();
            PieGraphLabel = new Label();
            LegendEntry = new StackPanel { Background = Brushes.Transparent };
            Repository = repository;
            slices = CreatePieSlices();
            AddPieSlicesToCanvas(slices);
            AddPieChart(slices);
        }

        public IDbOperations Repository { get; }
        public List<Pie> slices { get; set; }
        public StackPanel LegendEntry { get; private set; }
        public Label PieGraphLabel { get; private set; }
        public Grid PieGraph { get; private set; }

        private List<Pie> CreatePieSlices()
        {
            var pieChartData = new PieChart(Repository).PieChartData;
            var slices = new List<Pie>();
            var colors = GetListOfColors();

            double startAngle = 0;

            foreach (var item in pieChartData)
            {
                var slice = new Pie
                {
                    Tag = item.Category,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = 170,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 170,
                    Mode = PieMode.Slice,
                };

                slice.Fill = colors[pieChartData.IndexOf(item)];
                slice.Slice = (double)item.CategoryExpenditurePercentage;
                slice.StartAngle = startAngle;
                slices.Add(slice);

                startAngle = slices.Last().EndAngle;
            }
            return slices;
        }
        public List<SolidColorBrush> GetListOfColors()
        {

            var listOfBrushes = new List<SolidColorBrush> {
                new SolidColorBrush(Colors.LightPink),
                new SolidColorBrush(Colors.LightGray),
                new SolidColorBrush(Colors.LightGreen),
                new SolidColorBrush(Colors.LightBlue),
                new SolidColorBrush(Colors.Chocolate),
                new SolidColorBrush(Colors.Khaki),
                new SolidColorBrush(Colors.DarkMagenta),
                new SolidColorBrush(Colors.Purple) };

            return listOfBrushes;
        }

        private void AddPieChart(List<Pie> slices)
        {
            foreach (var slice in slices)
            {
                var newLegendEntry = new StackPanel();
                newLegendEntry.Orientation = Orientation.Horizontal;
                newLegendEntry.Background = Brushes.Transparent;

                newLegendEntry.Children.Add(new Rectangle
                {
                    Height = 12,
                    Width = 12,
                    Fill = slice.Fill,
                    Margin = new Thickness(5, 0, 5, 0)
                });

                newLegendEntry.Children.Add(new TextBlock
                {
                    Text = $"%{ (slice.Slice * 100).ToString("0.##")} -  {(string)slice.Tag}",
                    TextAlignment = TextAlignment.Left,
                    FontSize = 16,
                    FontFamily = new FontFamily("Helvetica")
                });

                LegendEntry.Children.Add(newLegendEntry);
            }
        }
        private void AddPieSlicesToCanvas(List<Pie> pieSlices)
        {

            PieGraphLabel.Content = "Yearly Expenses by Category";
            PieGraphLabel.FontSize = 22;

            foreach (var slice in pieSlices)
            {
                PieGraph.Children.Add(slice);
            }
        }
    }
}
