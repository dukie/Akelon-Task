using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributionApp
{
    class Distributor
    {
        protected Decimal sumForDistribution;
        protected List<Decimal> sumList;
        protected string method;
        protected Decimal sumListTotal;

        public Distributor(string method, double sum, string sumList)
        {
            this.method = method;
            this.sumForDistribution = Decimal.Round(new Decimal(sum), 2);
            this.sumList = sumList.Split(';').Select(n => Convert.ToDecimal(n)).ToList();
            this.sumListTotal = this.sumList.Sum();
        }

        protected List<Decimal> proportionalMethod()
        {
            List<Decimal> result = new List<Decimal>();
            Decimal residue = this.sumForDistribution;
            Decimal resultItem;
            foreach (Decimal item in this.sumList)
            {
                resultItem = Decimal.Round(item / this.sumListTotal * this.sumForDistribution , 2);
                result.Add(resultItem);
                residue -= resultItem;
            }
            result[result.Count - 1] += residue;
            return result;
        }


        protected List<Decimal> firstSignificantMethod()
        {
            List<Decimal> result = new List<Decimal>();
            Decimal residue = this.sumForDistribution;
            Decimal resultItem;
            foreach (Decimal item in this.sumList)
            {
                if (residue > 0)
                {
                    resultItem = residue >= item ? item : residue;
                    residue -= resultItem;
                }
                else resultItem = new Decimal(0);
                result.Add(resultItem);
            }
            return result;
        }


        protected List<Decimal> lastSignificantMethod()
        {
            List<Decimal> result;
            this.sumList.Reverse();
            result = firstSignificantMethod();
            result.Reverse();
            return result;
           
        }

        public string calculate()
        {
            List<Decimal> result;
            switch (this.method)
            { 
                case "ПEРВ":
                    result = firstSignificantMethod();
                    break;
                case "ПОСЛ":
                    result = lastSignificantMethod();
                    break;
                case "ПРОП":             
                default:
                    result = proportionalMethod();
                    break;
            }
            return String.Join(";", result);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Distributor calc = new Distributor("ПОДЛ", 10000.00, "1000;2000;3000;5000;8000;5000");
            System.Console.WriteLine(calc.calculate());
            calc = new Distributor("ПEРВ", 10000.00, "1000;2000;3000;5000;8000;5000");
            System.Console.WriteLine(calc.calculate());
            calc = new Distributor("ПОСЛ", 10000.00, "1000;2000;3000;5000;8000;5000");
            System.Console.WriteLine(calc.calculate());
            System.Console.ReadKey(true);
              
        }
    }
}
