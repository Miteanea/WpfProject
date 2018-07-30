using PersonalShopper.Db;
using PersonalShopper.Models;
using PersonalShopper.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace PersonalShopper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var config = CreateConfiguration();

            Repository = new DbOperations(config);
            
            CreatePieChart();
            CreateBarGraph();
        }
        public IDbOperations Repository { get; set; }
        private static IConfiguration CreateConfiguration()
        {
            Configuration.Instance.SetConnectionString(
               "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return Configuration.Instance;
        }
        private void CreateBarGraph()
        {
            BorderGraph.Child = new BarGraphModel(Repository).BarGraphCanvas;
        }
        private void CreatePieChart()
        {
            var pie = new PieChartModel(Repository);
            PieGraph.Children.Add(pie.PieGraph);
            LegendEntryGrid.Children.Add(pie.LegendEntry);
            PieGraphLabel.Content = pie.PieGraphLabel.Content;

        }

        private void ExpanderView(object sender, RoutedEventArgs e)
        {
            if (ExpanderStack.Children.Count == 0)
            {
                var backBtn = new Button { Content = "Back", Name = "backBtn" };
                backBtn.Click += Back_Btn_Click;

                var listCategories = Repository.GetCategories();
                var slices = new PieChartModel(Repository).slices;

                foreach (var item in listCategories)
                {
                    var expander = new ExpanderModel(item, MatchColor(item, slices),Repository).Expander;

                    ExpanderStack.Children.Add(expander);
                }
                ExpanderStack.Children.Add(backBtn);
            }
            if (ExpanderStack.Visibility == Visibility.Hidden)
            {
                ExpanderStack.Visibility = Visibility.Visible;
                ScrollView.Visibility = Visibility.Visible;
                PieChartGrid.Visibility = Visibility.Hidden;
                BorderGraph.Visibility = Visibility.Hidden;
            }
        }
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ExpanderStack.Visibility = Visibility.Hidden;
            ScrollView.Visibility = Visibility.Hidden;
            PieChartGrid.Visibility = Visibility.Visible;
            BorderGraph.Visibility = Visibility.Visible;
        }        
 
        private Brush MatchColor(string category, List<Pie> pies)
        {
            Brush color = Brushes.Transparent;
            foreach (var item in pies)
            {
                if (category == (string)item.Tag)
                {
                    return color = item.Fill;
                }
            }
            return color;
        }
        public static List<SolidColorBrush> GetListOfColors()
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


