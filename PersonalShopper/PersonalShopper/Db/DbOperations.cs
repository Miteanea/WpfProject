using PersonalShopper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

        internal List<Expense> DisplayExpnesePage(int pageNumber, string category)
        {
            var list = new List<Expense>();
            var upper = pageNumber * 10;
            var lower = upper - 9;

            using (var conn = DbConfiguration.CreateConnection())
            using (var comm = conn.CreateCommand())
            {
                conn.Open();

                comm.CommandType = System.Data.CommandType.Text;
                comm.CommandText =
                                        "with[Page] as(" +                                        
                                        "select[Id], [Name], [Quantity], [Price], [Date], [Category],"+
                                        "ROW_NUMBER() OVER(ORDER BY Date) AS[RowNumber] from[Expenses]"+
                                        $"where[Category] = '{category}')" +
                                        "select[Id], [Name], [Quantity], [Price], [Date], [Category], [RowNumber] from[Page]"+
                                        $"where[RowNumber] BETWEEN {lower} AND {upper};";

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var exp = new Expense
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetDecimal(2),
                            Price = reader.GetDecimal(3),
                            Date = reader.GetDateTime(4),
                            Category = reader.GetString(5),
                        };
                        exp.Sum =Math.Round( exp.Price * exp.Quantity, 2);
                        list.Add(exp);
                    }
                }
            }
            return list;
        }

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
                        var exp = new Expense
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetDecimal(2),
                            Price = reader.GetDecimal(3),
                            Date = reader.GetDateTime(4),
                            Category = reader.GetString(5),
                        };

                        exp.Sum = Math.Round(exp.Price * exp.Quantity, 2);
                        list.Add(exp);
                    }
                }
            }
            return list;
        }
       
        public List<string> GetCategories()
        {
            var list = GetExpenses().GroupBy(x => x.Category).Select(y => y.Key).ToList();
            return list;
        }

        public int GetNumberOfButtons(string category)
        {
            int expensesPerCategory = GetExpenses().Where(x => x.Category == category).Count();

            int tot = expensesPerCategory / 10;
            if (expensesPerCategory % 10 > 0)
            {
                tot++;
            }
                                    
            return tot;
        }

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
