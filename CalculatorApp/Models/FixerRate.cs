using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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

        public double Convert(double amount)
        {
            return amount * Rate;
        }
    }
}