﻿namespace CalculatorApp.Helpers
{
    public class CalculatorHelper
    {
        private readonly string[] numbers;
        private string symbol;
        private string calcText;

        public CalculatorHelper()
        {
            numbers = new string[2];
        }

        /// <summary>
        /// <param name="text">Button Text</param>
        /// Main Function for Calculation
        /// </summary>
        public string Calculation(string text)
        {
            if ("0123456789.".Contains(text))
                BuildDigits(text);
            else if ("÷+-*".Contains(text))
                BuildOperator(text);
            else if (text.Equals("="))
                Calculate();
            else if (text.Equals("DEL"))
                DeleteLastDigit();
            else if (text.Equals("+/-"))
                ChangeSign();
            else
                ClearText();

            return calcText;
        }

        /// <summary>
        /// Changes the sign of the last number in numbers array
        /// </summary>
        private void ChangeSign()
        {
            int index = symbol == null ? 0 : 1;
            numbers[index] = !string.IsNullOrEmpty(numbers[index]) ? (double.Parse(numbers[index]) * (-1)).ToString() : null;
            UpdateCalculator();
        }

        /// <summary>
        /// Clears the calculator text
        /// </summary>
        private void ClearText()
        {
            numbers[0] = numbers[1] = null;
            symbol = null;
            UpdateCalculator();
        }

        /// <summary>
        /// Deletes the last character inserted in the calculator text area
        /// </summary>
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

        /// <summary>
        /// <param name="newSymbol">Optional Parameter newSymbol.</param>
        /// Calculates the result. If newSymbol != null inserts newSymbol as current symbol
        /// </summary>
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

        /// <summary>
        /// <param name="digit">The Operator pressed.</param>
        /// Inserts the operator pressed in the calculator text area
        /// </summary>
        private void BuildOperator(string digit)
        {
            if (!string.IsNullOrEmpty(numbers[1]))
            {
                Calculate();
                symbol = digit;
                UpdateCalculator();
                return;
            }

            if (string.IsNullOrEmpty(numbers[0]))
                numbers[0] = "0";

            symbol = digit;
            UpdateCalculator();
        }

        /// <summary>
        /// <param name="digit">The Number/Digit pressed.</param>
        /// Inserts the number pressed in the calculator text area
        /// </summary>
        private void BuildDigits(string digit)
        {
            int index = symbol == null ? 0 : 1;

            if (digit.Equals(".") && numbers[index] != null && numbers[index].Contains("."))
                return;

            if (digit.Equals(".") && string.IsNullOrEmpty(numbers[index]))
                digit = "0.";

            numbers[index] += digit;

            UpdateCalculator();
        }

        /// <summary>
        /// Updates the Calculator Screen
        /// </summary>
        private void UpdateCalculator() => calcText = $"{numbers[0]} {symbol} {numbers[1]}";

    }
}