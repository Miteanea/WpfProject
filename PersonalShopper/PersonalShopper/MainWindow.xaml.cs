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
        public static MainWindow AppWindow;
        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;
            var config = CreateConfiguration();
            Repository = new DbOperations(config);

            CreatePieChart();
            CreateBarGraph();
            CreateExpanderView();
        }

        public IDbOperations Repository { get; set; }
        private PieChartModel pie { get; set; }
        private static IConfiguration CreateConfiguration()
        {
            Configuration.Instance.SetConnectionString(
               "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return Configuration.Instance;
        }
        private List<ExpanderModel> expanderModels { get; set; }

        private void UpdateGraphs(object sender, RoutedEventArgs e)
        {
            UpdateBarGraph();
            UpdatePieChart();
        }
        public void CreateBarGraph()
        {
            BorderGraph.Child = new BarGraphModel(Repository).BarGraphCanvas;
        }
        private void UpdateBarGraph()
        {
            CreateBarGraph();
        }
        private void CreatePieChart()
        {
            pie = new PieChartModel(Repository);
            PieGraph.Children.Add(pie.PieGraph);
            LegendEntryGrid.Children.Add(pie.LegendEntry);
            PieGraphLabel.Content = pie.PieGraphLabel.Content;
        }
        private void UpdatePieChart()
        {
            PieGraph.Children.Clear();
            LegendEntryGrid.Children.Clear();

            CreatePieChart();
        }
        public void CreateExpanderView()
        {
            expanderModels = new List<ExpanderModel>();
            if (ExpanderStack.Children.Count == 0)
            {
                var listCategories = Repository.GetCategories();
                var slices = new PieChartModel(Repository).slices;

                foreach (var item in listCategories)
                {
                    var expander = new ExpanderModel(item, MatchColor(item, slices), Repository);
                    expanderModels.Add(expander);
                    
                    ExpanderStack.Children.Add(expander.Expander);
                }
            }
        }
        public void UpdateExpanderView(string category)
        {
            foreach (var item in expanderModels)
            {
                if (item.Name == category)
                {
                    item.UpdateExpander();
                }
            }
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
      
    }

}


