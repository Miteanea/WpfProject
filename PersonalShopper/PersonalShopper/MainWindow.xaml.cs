using PersonalShopper.Db;
using System.Windows;

namespace PersonalShopper
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            DbOperations.Instance.DbConfiguration.SetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var userDataEntryWindow = new UserDataEntryWindow();
            userDataEntryWindow.Show();
        }
    }
}


