using PersonalShopper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PersonalShopper.Db
{
    public interface IDbOperations
    {
        List<Expense> GetExpenses();
        void AddExpense(Expense newExp);
    }

        

    public class DbOperations : IDbOperations
    {
        public IConfiguration DbConfiguration { get; set; }

        public List<Expense> GetExpenses()
        {
            var list = new List<Expense>();

            using (var conn = DbConfiguration.CreateConnection())
            using (var comm = conn.CreateCommand())
            {
                conn.Open();

                comm.CommandType = System.Data.CommandType.Text;
                comm.CommandText = "select * from [Expenses]" +
                    "order by [Date]";

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Expense
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetDecimal(2),
                            Price = reader.GetDecimal(3),
                            Date = reader.GetDateTime(4),
                            Category = reader.GetString(5),
                        });
                    }
                }
            }
            return list;
        }

        public List<String> GenerateComboBox()
        {
            List<string> comboList = new List<string> { newCategoryMessage };

            using (var conn = DbConfiguration.CreateConnection())
            using (var comm = conn.CreateCommand())
            {
                conn.Open();

                comm.CommandType = System.Data.CommandType.Text;
                comm.CommandText = "select Category from [expenses] group by Category order by Category ";

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboList.Add(reader.GetString(0));
                    }
                }
            }
            return comboList;
        }

        public static string newCategoryMessage = "Create New Category";

        public void AddExpense(Expense newExp)
        {
            using (var conn = DbConfiguration.CreateConnection())
            using (var comm = conn.CreateCommand())
            {
                conn.Open();

                comm.CommandType = System.Data.CommandType.Text;
                comm.CommandText = " insert into [Expenses]" +
                                     "(Name, quantity, price, date, category)" +
                                     "values" +
                                    $"('{newExp.Name}', {newExp.Quantity}, {newExp.Price}, '{newExp.Date.ToString("yyyy/MM/dd hh:mm:ss")}', '{newExp.Category}')";

                comm.ExecuteNonQuery();
            }
        }

        #region Singleton
        private DbOperations()
        {
        }

        static DbOperations()
        {
            Instance = new DbOperations();
            Instance.DbConfiguration = Configuration.Instance;
        }

        public static DbOperations Instance { get; }
        #endregion

    }

    public interface IConfiguration
    {
        void SetConnectionString(string value);
        SqlConnection CreateConnection();
    }

    public class Configuration : IConfiguration
    {
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void SetConnectionString(string value)
        {
            _connectionString = value;
        }
        private string _connectionString { get; set; }

        #region Singleton
        private Configuration() { }

        static Configuration()
        {
            Instance = new Configuration();
        }

        public static Configuration Instance { get; }
        #endregion
   
    }

}
