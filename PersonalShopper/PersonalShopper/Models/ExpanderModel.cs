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
            Name = item;
            _pageNumber = 1;
            Repository = repository;
            _listOfButtons = new List<Button>();
            Expander = new Expander
            { Header = item, Background = color };
            GenerateExpanderBody(Expander);
        }

        private StackPanel _btnPanel { get; set; }
        private List<Button> _listOfButtons { get; set; }
        private DataGrid _dataGrid { get; set; }
        private Grid _expanderGrid { get; set; }
        private int _pageNumber { get; set; }
        private StackPanel _addBtnStack { get; set; }
        private StackPanel _centerButtonsPanel { get; set; }
        public int numberOfButtons { get; set; }
        public string Name { get; set; }
        public IDbOperations Repository { get; }
        public Expander Expander { get; set; }


        private void GenerateExpanderBody(Expander expander)
        {
            expander.Content = CreateExpanderContentGrid();
            DisplayExpenses(_dataGrid, _pageNumber, Name);
        }

        private Grid CreateExpanderContentGrid()
        {
            var expanderGrid = new Grid();

            ConfigureGrid(expanderGrid);

            numberOfButtons = Repository.GetNumberOfButtons(Name);

            CreateButtons(numberOfButtons);

            return expanderGrid;
        }
        private void ConfigureGrid(Grid expanderGrid)
        {
            expanderGrid.RowDefinitions.Add(new RowDefinition());
            expanderGrid.RowDefinitions.Add(new RowDefinition());
            expanderGrid.RowDefinitions.Add(new RowDefinition());

            expanderGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _dataGrid = new DataGrid { HorizontalAlignment = HorizontalAlignment.Center, MinWidth = 500 };
            expanderGrid.Children.Add(_dataGrid);
            Grid.SetRow(_dataGrid, 0);
            Grid.SetColumn(_dataGrid, 0);

            _centerButtonsPanel = new StackPanel { Orientation = Orientation.Vertical };
            _btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            expanderGrid.Children.Add(_centerButtonsPanel);
            Grid.SetRow(_centerButtonsPanel, 1);
            Grid.SetColumn(_centerButtonsPanel, 0);

            _addBtnStack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            expanderGrid.Children.Add(_addBtnStack);
            Grid.SetRow(_addBtnStack, 2);
            Grid.SetColumn(_addBtnStack, 0);

            _centerButtonsPanel.Children.Add(_btnPanel);
        }
        private void CreateButtons(int tot)
        {
            for (int i = 1; i <= tot; i++)
            {
                var btn = new Button { Name = $"btn{i}", Content = $"{i}", Width = double.NaN, Height = double.NaN };
                btn.Click += new RoutedEventHandler(Page_Btn_Click);
                _listOfButtons.Add(btn);
            }
            _listOfButtons[0].FontWeight = FontWeights.Heavy;

            foreach (var btn in _listOfButtons)
            {
                _btnPanel.Children.Add(btn);
            }

            var addBtn = new Button { Content = "Add Expense", HorizontalAlignment = HorizontalAlignment.Left };
            _addBtnStack.Children.Add(addBtn);
            addBtn.Click += new RoutedEventHandler(Add_Btn_Click);
        }

        private void DisplayExpenses(DataGrid dataGrid, int pageNumber, string category)
        {
            dataGrid.ItemsSource = Repository.DisplayExpensePage(pageNumber, category);
        }
        private void Page_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var btn in _listOfButtons)
            {
                btn.FontWeight = FontWeights.Light;
            }

            var thisBtn = (Button)sender;
            thisBtn.FontWeight = FontWeights.Heavy;

            var pageNumber = int.Parse(((Button)sender).Content.ToString());

            DisplayExpenses(_dataGrid, pageNumber, Name);
        }
        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            var userDataEntryWindow = new UserDataEntryWindow(Name, Repository);
            userDataEntryWindow.Show();
        }

        public void UpdateExpander()
        {
            UpdateDatagrid();
            UpdateButtons();
        }

        private void UpdateDatagrid()
        {
            DisplayExpenses(_dataGrid, _pageNumber, Name);
        }
        private void UpdateButtons()
        {
            _btnPanel.Children.Clear();
            _listOfButtons.Clear();
            numberOfButtons = Repository.GetNumberOfButtons(Name);

            for (int i = 1; i <= numberOfButtons; i++)
            {
                var btn = new Button { Name = $"btn{i}", Content = $"{i}", Width = double.NaN, Height = double.NaN };
                btn.Click += new RoutedEventHandler(Page_Btn_Click);
                _listOfButtons.Add(btn);
            }
            _listOfButtons[0].FontWeight = FontWeights.Heavy;

            foreach (var btn in _listOfButtons)
            {
                _btnPanel.Children.Add(btn);
            }
        }
    }
}
