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

        public List<MonthlyExpense> GenerateBarGraphData()
        {
            var date = DateTime.Now;
            var cutoffDate = date.AddYears(-1);
            var _expenses = Repository.GetExpenses();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var list =
                _expenses.Where(d => d.Date > cutoffDate && d.Date < firstDayOfMonth)
                .GroupBy(x => x.Date.ToString("MMM/yy"))
                        .Select(y => new MonthlyExpense
                        {
                            Month = y.Key,
                            MonthlyExpenditure = (y.Sum(s => s.Price * s.Quantity)),
                        })
                        .ToList();
            return list;
        }
    }
}
