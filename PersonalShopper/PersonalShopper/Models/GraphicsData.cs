using PersonalShopper.Db;
using System.Collections.Generic;
using System.Linq;

namespace PersonalShopper.Models
{
    public class PieChart
    {
        public List<CategoryExpensePercentage> PieChartData { get { return GeneratePieChartData(); } }
        public List<CategoryExpensePercentage> GeneratePieChartData()
        {
            var _expenses = DbOperations.Instance.GetExpenses();
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

        public List<MonthlyExpense> MonthlyExpenseBarGraphData { get; }
        public List<MonthlyExpense> GenerateBarGraphData()
        {
            var _expenses = DbOperations.Instance.GetExpenses();
            var list =
                _expenses.GroupBy(x => x.Date.Month)
                        .Select(y => new MonthlyExpense
                        {
                            Month = y.Key,
                            MonthlyExpenditure = (y.Sum(s => s.Price * s.Quantity) ),
                        })
                        .ToList();

            //Be careful with order of records
            return list;
        }
    }

    public class ExpanderView
    {
        
    }

}
