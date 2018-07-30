using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomDataGeneratorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var count = 0;


            var dataList = new List<ExpenseData>();

            var seedData = new SeedData();

            string path = "e:\\repos\\wpfproject\\sampledata\\sampledata200.txt";

            while (count < 200)
            {
                dataList.Add(new ExpenseData(seedData));
                count++;
            }
           
            foreach (var item in dataList)
            {
                File.AppendAllText(path, item.ToString());
            }            
        }
    }

    class ExpenseData
    {
        public ExpenseData(SeedData seedData)
        {
            var rnd = new Random();

            Category = seedData.Categories[rnd.Next(seedData.Categories.Count)].CategoryName;
            Name = seedData.Categories.
                Find(x => x.CategoryName == Category)
                .ProductName[
                rnd.Next(
                    seedData.Categories
                    .Find(
                        x => x.CategoryName == Category)
                        .ProductName.Count)];
            Quantity = seedData.RangeOneToTen[rnd.Next(seedData.RangeOneToTen.Count)];
            Price = seedData.PriceRange100[rnd.Next(seedData.PriceRange100.Count)];
            Date = seedData.TimeRange[rnd.Next(seedData.TimeRange.Count)];
            Sum = Quantity * Price;
        }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public decimal Sum { get; }

        public override string ToString()
        {
            return $"('{Name}', {Quantity}, {Price}, '{Date.ToString("yyyy/MM/dd hh:mm:ss")}', '{Category}'), \r\n";
        }
    }

    class SeedData
    {
        public SeedData()
        {
            Categories = new List<Category> {
                new Category { CategoryName="Alcohol(0)",
                               ProductName = new List<string>{"prod01","prod02","prod03","prod04","prod05" } },
                new Category { CategoryName="Clothes(1)",
                               ProductName = new List<string>{"prod11","prod12","prod13","prod14","prod15" } },
                new Category { CategoryName="Books(2)",
                               ProductName = new List<string>{"prod21","prod22","prod23","prod24","prod25" } },
                new Category { CategoryName="Food(3)",
                               ProductName = new List<string>{"prod31","prod32","prod33","prod34","prod35" } },
                new Category { CategoryName="Electronics(4)",
                               ProductName = new List<string>{"prod41","prod42","prod43","prod44","prod45" } },
                new Category { CategoryName="Games(5)",
                               ProductName = new List<string>{"prod51","prod52","prod53","prod54","prod55" } }
            };
            RangeOneToTen = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            PriceRange100 = CreateOneToHundredRange();
            TimeRange = CreateTimeRange();
        }

        private List<decimal> CreateOneToHundredRange()
        {
            var list = new List<decimal>();
            decimal step = 0.1m;
            decimal cumulative = 0.09m;

            while (cumulative <= 100)
            {
                list.Add(cumulative);
                cumulative += step;
            }
            return list;
        }
        private List<DateTime> CreateTimeRange()
        {
            var list = new List<DateTime>();

            var startTime = new DateTime(2017, 5, 1);

            while (startTime <= DateTime.Now)
            {
                if (8 <= startTime.Hour && startTime.Hour <= 21)
                {
                    list.Add(startTime);
                }
                startTime = startTime.AddHours(2);
            }
            return list;
        }


        public List<string> Names { get; }
        public List<int> RangeOneToTen { get; }
        public List<decimal> PriceRange100 { get; }
        public List<DateTime> TimeRange { get; }
        public List<Category> Categories { get; }
    }

    class Category
    {
        public string CategoryName { get; set; }
        public List<string> ProductName { get; set; }
    }
}
