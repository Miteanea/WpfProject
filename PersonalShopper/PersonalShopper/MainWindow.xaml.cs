using PersonalShopper.Db;
using PersonalShopper.Models;
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

            DataContext = this;

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
            var barGraphData = new MonthlyChart(Repository).MonthlyExpenseData;
            
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

                Canvas.SetLeft(label, init);
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

        private void CreatePieChart()
        {
            List<Pie> pieSlices;

            pieSlices = CreatePieSlices();


            AddPieSlicesToCanvas(pieSlices);
            AddPieChart(pieSlices);
        }
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

        private void ExpanderView(object sender, RoutedEventArgs e)
        {
            if (ExpanderStack.Children.Count == 0)
            {
                var backBtn = new Button { Content = "Back", Name = "backBtn" };
                backBtn.Click += Back_Btn_Click;

                var listCategories = Repository.GetCategories();
                var pies = CreatePieSlices();

                foreach (var item in listCategories)
                {
                    var expander = new Expander { Header = item, Background = MatchColor(item, pies) };

                    ExpanderStack.Children.Add(expander);
                    GenerateExpanderBody(expander);
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
        private void GenerateExpanderBody(Expander expander)
        {
            var expanderGrid = new Grid();

            expander.Content = expanderGrid;
            var category = expander.Header.ToString();

            expanderGrid.RowDefinitions.Add(new RowDefinition());
            expanderGrid.RowDefinitions.Add(new RowDefinition());
            expanderGrid.RowDefinitions.Add(new RowDefinition());

            expanderGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var addBtnStack = new StackPanel();
            expanderGrid.Children.Add(addBtnStack);
            Grid.SetRow(addBtnStack, 0);
            Grid.SetColumn(addBtnStack, 0);

            var addBtn = new Button { Content = "Add Expense", HorizontalAlignment = HorizontalAlignment.Left };
            addBtnStack.Children.Add(addBtn);
            addBtn.Click += new RoutedEventHandler(Add_Btn_Click);

            int pageNumber = 1;
            var currentPage = new Label { Content = "Page: " + pageNumber.ToString(), FontSize = 16 };
            currentPage.HorizontalAlignment = HorizontalAlignment.Right;
            addBtnStack.Children.Add(currentPage);

            var dataGrid = new DataGrid();
            expanderGrid.Children.Add(dataGrid);
            Grid.SetRow(dataGrid, 1);
            Grid.SetColumn(dataGrid, 0);


            var centerButtonsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            expanderGrid.Children.Add(centerButtonsPanel);
            Grid.SetRow(centerButtonsPanel, 2);
            Grid.SetColumn(centerButtonsPanel, 0);

            centerButtonsPanel.Children.Add(btnPanel);

            var tot =Repository.GetNumberOfButtons(category);

            for (int i = 1; i <= tot; i++)
            {
                var btn = new Button { Name = $"btn{i}", Content = $"{i}", Width = double.NaN, Height = double.NaN };
                btn.Click += new RoutedEventHandler(Page_Btn_Click);

                btnPanel.Children.Add(btn);
            }

            DisplayExpenses(dataGrid, pageNumber, expander.Header.ToString());     //returns a list of first 10 expenses in this category
        }

        
        private void DisplayExpenses(DataGrid dataGrid, int pageNumber, string category)
        {
            dataGrid.ItemsSource = Repository.DisplayExpensePage(pageNumber, category);
        }
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ExpanderStack.Visibility = Visibility.Hidden;
            ScrollView.Visibility = Visibility.Hidden;
            PieChartGrid.Visibility = Visibility.Visible;
            BorderGraph.Visibility = Visibility.Visible;

        }
        private void Page_Btn_Click(object sender, RoutedEventArgs e)
        {
            var thisBtn = (Button)sender;
            var pageNumber = int.Parse(thisBtn.Content.ToString());

            var BtnsPanel = (StackPanel)(thisBtn).Parent;
            var centerBtnsPanel = (StackPanel)((StackPanel)BtnsPanel.Parent);
            var grid = (Grid)(((StackPanel)centerBtnsPanel).Parent);
            var dataGrid = (DataGrid)grid.Children[1];

            var expander = (Expander)grid.Parent;
            var new1 = (Grid)expander.Content;
            var stack1 = (StackPanel)new1.Children[0];
            var currentPageLabel = (Label)stack1.Children[1];

            currentPageLabel.Content = "Page: " + pageNumber.ToString();
            DisplayExpenses(dataGrid, pageNumber, expander.Header.ToString());
            //ReasignButtons(pageNumber, totalPages, BtnsPanel);
        }
        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement parent = (FrameworkElement)((Button)sender).Parent;
            FrameworkElement parentOfParent = (FrameworkElement)((StackPanel)parent).Parent;
            Expander parentOfParentOfParent = (Expander)((Grid)parentOfParent).Parent;

            var userDataEntryWindow = new UserDataEntryWindow(parentOfParentOfParent.Header.ToString(), Repository);

            userDataEntryWindow.Show();
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
    enum Months
    {
        Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec,
    }
}


