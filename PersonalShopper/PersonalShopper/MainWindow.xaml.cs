using PersonalShopper.Db;
using PersonalShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace PersonalShopper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            DbOperations.Instance.DbConfiguration.SetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            CreatePieChart();
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


                var newLegendEntry = new System.Windows.Controls.StackPanel();
                newLegendEntry.Orientation = System.Windows.Controls.Orientation.Horizontal;
                

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

            return slices;
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
                new SolidColorBrush(Colors.Beige),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Chocolate),
                new SolidColorBrush(Colors.Coral),
                new SolidColorBrush(Colors.DarkGoldenrod),
                new SolidColorBrush(Colors.DarkMagenta),
                new SolidColorBrush(Colors.Purple) };

            return listOfBrushes;
        }

    }
}


