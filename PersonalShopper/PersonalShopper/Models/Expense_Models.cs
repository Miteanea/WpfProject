using System;

namespace PersonalShopper.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public decimal Sum { get; set; }
    }

    public class MonthlyExpense
    {   
        public string Month { get; set; }
        public decimal MonthlyExpenditure { get; set; }       
    }

    public class CategoryExpensePercentage
    {
        public string Category { get; set; }
        public decimal CategoryExpenditurePercentage { get; set; }
    }
}
