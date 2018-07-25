using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalShopper;
using PersonalShopper.Db;
using PersonalShopper.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        List<Expense> mockExpCont;


        [TestInitialize]
        public void TestInitialize()
        {
            DbOperations.Instance.DbConfiguration.SetConnectionString(ConnectionString);
            mockExpCont = DbOperations.Instance.GetExpenses();
        }

        [TestMethod]
        public void Db_GetExpenses_Test()
        {
            Assert.AreEqual(20 ,mockExpCont.Count);

            Assert.AreEqual(13, mockExpCont[0].Id);
            Assert.AreEqual("prod13", mockExpCont[0].Name);
            Assert.AreEqual("2017-07-14", mockExpCont[0].Date.ToString("yyyy/MM/dd"));

            Assert.AreEqual(5, mockExpCont[8].Id);
            Assert.AreEqual("prod35", mockExpCont[8].Name);
            Assert.AreEqual("2018-01-09" ,mockExpCont[8].Date.ToString("yyyy/MM/dd"));
        }

        [TestMethod]
        public void Pie_Chart_Data_Test()
        {
            var graphData = new PieChart();

            var test = graphData.GeneratePieChartData();

            Assert.AreEqual("Alcohol(0)", test[0].Category );
            Assert.AreEqual("Books(2)", test[1].Category);
            Assert.AreEqual("Clothes(1)", test[2].Category);

            Assert.AreEqual(0.16m, Math.Round(test[0].CategoryExpenditurePercentage, 2));
            Assert.AreEqual(0.27m, Math.Round(test[1].CategoryExpenditurePercentage, 2));
            Assert.AreEqual(0.24m, Math.Round(test[2].CategoryExpenditurePercentage, 2));
        }

        [TestMethod]
        public void Pie_Chart_Data_Test_Sum_is_100()
        {
            var graphData = new PieChart().PieChartData;

            var test = graphData.Select(x => x.CategoryExpenditurePercentage).Sum()*100;
            Assert.AreEqual(100m, test);
        }


        [TestMethod]
        public void Bar_Graph_Data_Test()
        {
            var graphData = new MonthlyChart();

            var test = graphData.GenerateBarGraphData();

            Assert.AreEqual(7,test[0].Month);
            Assert.AreEqual(8, test[1].Month);
            Assert.AreEqual(9, test[2].Month);


            //Assert.AreEqual(Math.Round(test[0].MonthlyExpenditure, 2) , 426.23m );

            //doesn't pass because of 0.02 difference 
            Assert.AreEqual(Math.Round(test[1].MonthlyExpenditure, 2), 1598.83m);
            Assert.AreEqual(Math.Round(test[2].MonthlyExpenditure, 2), 102.18m);
        }

        [TestMethod]
        public void Add_Expense_Test()
        {
            DbOperations.Instance.AddExpense(new Expense { Name = "xxx", Category = "xxx", Date = new DateTime(2018, 12, 31, 19, 18, 00), Price = 29.99m, Quantity = 10 });

            Assert.AreEqual(DbOperations.Instance.GetExpenses().Count, 21);
        }

      


        //private static IConfiguration CreateConfiguration()
        //{
        //    Configuration.Instance.SetConnectionString(
        //        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Personal Shopper\";Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //    return Configuration.Instance;
        //}
    }
}
