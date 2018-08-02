using PersonalShopper.Db;
using PersonalShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonalShopper
{
    /// <summary>
    /// Interaction logic for UserDataEntryWindow.xaml
    /// </summary>
    public partial class UserDataEntryWindow : Window
    {   

        public UserDataEntryWindow( string value, IDbOperations repository)
        {
            InitializeComponent();

            Repository = repository;
            CategoryTextBox.Text = value;
        }        

        public IDbOperations Repository { get; }

        private void QuantityTextChanged(object sender, TextChangedEventArgs e)
        {
            QuantityBlock.Foreground = new SolidColorBrush(Colors.Red);
            UpdateSumBox();
        }

        private void PriceTextChanged(object sender, TextChangedEventArgs e)
        {
            PriceBlock.Foreground = new SolidColorBrush(Colors.Red);
            UpdateSumBox();
        }
        private void UpdateSumBox()
        {
            var goodP = decimal.TryParse(Price.Text, out decimal p);
            var goodQ = decimal.TryParse(Quantity.Text, out decimal q);

            CheckFormat(goodP, goodQ);

            if (goodP && goodQ)
            {
                SumBlock.Foreground = new SolidColorBrush(Colors.Green);
                SumBox.Text = (p * q).ToString();
            }
            else
            {
                SumBlock.Foreground = new SolidColorBrush(Colors.Red);
                SumBox.Text = "Calculating...";
            }
        }
        private void CheckFormat(bool goodP, bool goodQ)
        {
            if (goodP)
            {
                PriceBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            if (goodQ)
            {
                QuantityBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            ExpenseNameBlock.Foreground = new SolidColorBrush(Colors.Red);

            if (!string.IsNullOrWhiteSpace(NameBox.Text))
            {
                ExpenseNameBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void DateBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            DateBlock.Foreground = new SolidColorBrush(Colors.Red);

            var goodD = DateTime.TryParse(DateBox.Text, out DateTime expenseDate);

            if (goodD)
                DateBlock.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrWhiteSpace(NameBox.Text) &&
                DateTime.TryParse(DateBox.Text, out DateTime x) &&
                decimal.TryParse(Price.Text, out decimal y) &&
                decimal.TryParse(Quantity.Text, out decimal q))
            {
                var newExp = new Expense
                {
                    // (Name, quantity, price, date, category)
                    Name = NameBox.Text,
                    Quantity = decimal.Parse(Quantity.Text),
                    Price = decimal.Parse(Price.Text),
                    Date = DateTime.Parse(DateBox.Text),
                    Category = CategoryTextBox.Text
                };
                
                Repository.AddExpense(newExp);
                PersonalShopper.MainWindow.AppWindow.UpdateExpanderView( newExp.Category);

                MessageBox.Show("Expense Saved To Database!", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                
                this.Close();
            }
            else
            {
                MessageBox.Show("Not all Inputs are entered correctly", "Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
