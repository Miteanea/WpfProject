using PersonalShopper.Db;
using PersonalShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace PersonalShopper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            DbOperations.Instance.DbConfiguration.SetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            CreatePieChart();
            CreateBarGraph();

        }

        private void CreateBarGraph()
        {
            var barGraphData = new MonthlyChart().MonthlyExpenseData;

            AddBarsToView(barGraphData);
        }

        private void AddBarsToView(List<MonthlyExpense> barGraphData)
        {
            double init = 0;
            System.Windows.Shapes.Rectangle bar;
            foreach (var month in barGraphData)
            {
                bar = new System.Windows.Shapes.Rectangle
                {
                    Width = -10,
                    Height = -(double)month.MonthlyExpenditure / 10,
                    Margin = new Thickness(5, 5, 5, 5),
                    Fill = Brushes.Black,
                    VerticalAlignment = VerticalAlignment.Bottom,
                };
            }
            bar = new System.Windows.Shapes.Rectangle
            {
                Width = 1,

                Height = 1,

                Margin = new Thickness(5, 5, 5, 5),
                Fill = Brushes.Black
            };

            Canvas.SetLeft(bar, init);

            BarGraphCanvas.Children.Add(bar);
        }

        private void CreatePieChart()
        {
            List<Xceed.Wpf.Toolkit.Pie> pieSlices;

            pieSlices = CreatePieSlices();
            AddPieSlicesToCanvas(pieSlices);
        }

        private List<Pie> CreatePieSlices()
        {
            var pieChartData = new PieChart().PieChartData;
            var slices = new List<Xceed.Wpf.Toolkit.Pie>();
            var colors = GetListOfColors();

            double startAngle = 0;

            foreach (var item in pieChartData)
            {
                var slice = new Pie
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = 150,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 150,
                    Mode = PieMode.Slice,
                };

                slice.Fill = colors[pieChartData.IndexOf(item)];
                slice.Slice = (double)item.CategoryExpenditurePercentage;
                slice.StartAngle = startAngle;
                slices.Add(slice);

                startAngle = slices.Last().EndAngle;

                AddGraphLegend(item, slice);
            }
            return slices;
        }

        private void AddGraphLegend(CategoryExpensePercentage item, Pie slice)
        {
            var newLegendEntry = new System.Windows.Controls.StackPanel();
            newLegendEntry.Orientation = System.Windows.Controls.Orientation.Horizontal;
            newLegendEntry.Background = null;

            newLegendEntry.Children.Add(new System.Windows.Shapes.Rectangle
            {
                Height = 5,
                Width = 5,
                Fill = slice.Fill
            });

            newLegendEntry.Children.Add(new System.Windows.Controls.TextBlock
            {
                Text = $"%{ (item.CategoryExpenditurePercentage * 100).ToString("0.##")} -  {item.Category}",
                TextAlignment = TextAlignment.Left,
            }
                 );

            LegendEntry.Children.Add(newLegendEntry);
        }

        private void AddPieSlicesToCanvas(List<Pie> pieSlices)
        {
            foreach (var slice in pieSlices)
            {
                PieGraph.Children.Add(slice);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var userDataEntryWindow = new UserDataEntryWindow();
            userDataEntryWindow.Show();
        }

        public List<SolidColorBrush> GetListOfColors()
        {

            var listOfBrushes = new List<SolidColorBrush> {
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Gray),
                new SolidColorBrush(Colors.Green),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Chocolate),
                new SolidColorBrush(Colors.DarkGoldenrod),
                new SolidColorBrush(Colors.DarkMagenta),
                new SolidColorBrush(Colors.Purple) };

            return listOfBrushes;
        }

    }
}


