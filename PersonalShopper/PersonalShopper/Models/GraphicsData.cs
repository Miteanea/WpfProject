using PersonalShopper.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalShopper.Models
{
    public class PieChart
    {
        public PieChart(IDbOperations repository)
        {
            Repository = repository;
        }
        public IDbOperations Repository { get; }
        public List<CategoryExpensePercentage> PieChartData { get { return GeneratePieChartData(); } }
        private List<CategoryExpensePercentage> GeneratePieChartData()
        {
            var _expenses = Repository.GetExpenses();
            var list =
                _expenses.GroupBy(x => x.Category)
                        .Select(y => new CategoryExpensePercentage
                        {
                            Category = y.Key,
                            CategoryExpenditurePercentage = (y.Sum(p => p.Price * p.Quantity) / _expenses.Sum(f => f.Quantity*f.Price)),
                        })
                        .OrderBy(x=> x.Category)
                        .ToList();
            return list;
        }
    }

    public class MonthlyChart
    {
        public MonthlyChart(IDbOperations repository)
        {
            Repository = repository;
        }
        public IDbOperations Repository { get; }
        public List<MonthlyExpense> MonthlyExpenseData { get { return GenerateBarGraphData(); } }
        private List<MonthlyExpense> GenerateBarGraphData()
        {
            var cutoffDate = DateTime.Now.AddMonths(-12);
            var _expenses = Repository.GetExpenses();
            var list =
                _expenses.Where(d => d.Date >= cutoffDate)
                .GroupBy(x => x.Date.ToString("MMM/yy"))
                        .Select(y => new MonthlyExpense
                        {
                            Month = y.Key,
                            MonthlyExpenditure = (y.Sum(s => s.Price * s.Quantity)),
                        })
                        .ToList();

            //Be careful with order of records
            return list;
        }
    }

}
