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

namespace CalculatorApp.Activities
{
    [Activity(Label = "CalculatorActivity")]
    public class CalculatorActivity : Activity
    {
        private TextView calcText;
        private string[] numbers = new string[2];
        private string symbol;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            calcText = FindViewById<TextView>(Resource.Id.calculator_text_view);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;

            if ("0123456789.".Contains(button.Text))
                BuildDigits(button.Text);
            else if ("÷+-x".Contains(button.Text))
                BuildOperator(button.Text);
            else if (button.Text.Equals("="))
                Calculate();
            else if (button.Text.Equals("DEL"))
                DeleteLastDigit();
            else
                ClearText();
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
            numbers[index] = numbers[index].Remove(numbers[index].Length -1);

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
                case "x":
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
            if (numbers[1] != null)
            {
                Calculate();
                return;
            }

            symbol = digit;
            UpdateCalculator();
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