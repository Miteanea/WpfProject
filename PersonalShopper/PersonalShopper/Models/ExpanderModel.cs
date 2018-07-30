using PersonalShopper.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;

namespace PersonalShopper.Models
{
    class ExpanderModel
    {
        public ExpanderModel(string item, Brush color, IDbOperations repository)
        {
            Repository = repository;
            Expander = new Expander
            { Header = item, Background = color };
            GenerateExpanderBody(Expander);
        }

        public IDbOperations Repository { get; }
        public Expander Expander { get; set; }

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

            var tot = Repository.GetNumberOfButtons(category);

            for (int i = 1; i <= tot; i++)
            {
                var btn = new Button { Name = $"btn{i}", Content = $"{i}", Width = double.NaN, Height = double.NaN };
                btn.Click += new RoutedEventHandler(Page_Btn_Click);

                btnPanel.Children.Add(btn);
            }

            DisplayExpenses(dataGrid, pageNumber, expander.Header.ToString());
        }
        private void DisplayExpenses(DataGrid dataGrid, int pageNumber, string category)
        {
            dataGrid.ItemsSource = Repository.DisplayExpensePage(pageNumber, category);
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
        }
        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            var stack = (StackPanel)((Button)sender).Parent;
            var grid = (Grid)((StackPanel)stack).Parent;
            var expander = (Expander)((Grid)grid).Parent;

            var userDataEntryWindow = new UserDataEntryWindow(expander.Header.ToString(), Repository);

            userDataEntryWindow.Show();
        }
    }
}
