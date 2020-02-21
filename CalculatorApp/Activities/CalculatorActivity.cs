using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace CalculatorApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CalculatorActivity : AppCompatActivity
    {
        private TextView calcText;
        private readonly string[] numbers = new string[2];
        private string symbol;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Create your application here

            calcText = FindViewById<TextView>(Resource.Id.calculator_text_view);
        }

        public override void OnBackPressed()
        {
            return;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.currency)
            {
                StartActivity(typeof(CurrencyActivity));
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;

            if ("0123456789.".Contains(button.Text))
                BuildDigits(button.Text);
            else if ("÷+-*".Contains(button.Text))
                BuildOperator(button.Text);
            else if (button.Text.Equals("="))
                Calculate();
            else if (button.Text.Equals("DEL"))
                DeleteLastDigit();
            else if (button.Text.Equals("+/-"))
                ChangeSign();
            else
                ClearText();
        }

        private void ChangeSign()
        {
            int index = symbol == null ? 0 : 1;
            numbers[index] = (double.Parse(numbers[index]) * (-1)).ToString();
            UpdateCalculator();
        }

        private void ClearText()
        {
            numbers[0] = numbers[1] = null;
            symbol = null;
            UpdateCalculator();
        }

        private void DeleteLastDigit()
        {
            int index = symbol == null ? 0 : 1;
            if (!string.IsNullOrEmpty(numbers[index]))
            {
                numbers[index] = numbers[index].Remove(numbers[index].Length - 1);

                if (numbers[index] == "")
                    numbers[index] = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(symbol))
                    symbol = null;
            }

            UpdateCalculator();
        }

        private void Calculate(string newSymbol = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (symbol)
            {
                case "÷":
                    result = second != 0 ? first / second : null;
                    break;
                case "*":
                    result = first * second;
                    break;
                case "-":
                    result = first - second;
                    break;
                case "+":
                    result = first + second;
                    break;
            }

            if (result != null)
            {
                numbers[0] = result.ToString();
                symbol = newSymbol;
                numbers[1] = null;
                UpdateCalculator();
            }
        }

        private void BuildOperator(string digit)
        {
            if (!string.IsNullOrEmpty(numbers[1]))
            {
                Calculate();
                symbol = digit;
                UpdateCalculator();
                return;
            }

            if (!string.IsNullOrEmpty(numbers[0]))
            {
                symbol = digit;
                UpdateCalculator();
            }
        }

        private void BuildDigits(string digit)
        {
            int index = symbol == null ? 0 : 1;

            if (digit.Equals(".") && numbers[index].Contains("."))
                return;

            numbers[index] += digit;

            UpdateCalculator();
        }

        private void UpdateCalculator() => calcText.Text = $"{numbers[0]} {symbol} {numbers[1]}";

    }
}