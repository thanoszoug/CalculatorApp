using System;

namespace CalculatorApp.Models
{
    public class FixerRate
    {
        public FixerRate(string from, string to, double rate, DateTime date)
        {
            From = from;
            To = to;
            Rate = rate;
            Date = date;
        }

        public string From { get; }

        public string To { get; }

        public double Rate { get; }

        public DateTime Date { get; set; }

        /// <summary>
        /// <param name="amount">The requested amount</param>
        /// Converts the requested currency amount into the new converted amount
        /// </summary>
        public double Convert(double amount)
        {
            return amount * Rate;
        }
    }
}