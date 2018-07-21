using PersonalShopper.Db;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PersonalShopper
{
    /// <summary>
    /// Interaction logic for NewCategoryInputWindow.xaml
    /// </summary>
    public partial class NewCategoryInputWindow : Window
    {
        private UserDataEntryWindow ToUpdate { get; set; }

        public NewCategoryInputWindow()
        {
        }
        public NewCategoryInputWindow(UserDataEntryWindow comboBox)
        {
            InitializeComponent();
            ToUpdate = comboBox;
        }
        

        private void NewCategoryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
             NewCategoryNameBlock.Foreground = new SolidColorBrush(Colors.Red);
            if (!string.IsNullOrWhiteSpace(NewCategoryNameBox.Text))
            {
                NewCategoryNameBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            var list = DbOperations.Instance.GenerateComboBox();
            list.Add(NewCategoryNameBox.Text);
            ToUpdate.CategoryComboBox.ItemsSource = list;
            ToUpdate.CategoryComboBox.SelectedItem = list.Last();
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
