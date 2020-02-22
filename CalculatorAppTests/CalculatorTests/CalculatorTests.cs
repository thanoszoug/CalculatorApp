using Android.Support.V7.App;
using Android.Widget;
using CalculatorApp.Activities;
using CalculatorApp.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorAppTests.CalculatorTests
{
    [TestFixture]
    class CalculatorTests
    {

        [Test]
        public void NoFirstDigitTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("-");
            Assert.That(calcText == "0 - ");

            calcText = calcHelper.Calculation("*");
            Assert.That(calcText == "0 * ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "0 + ");

            calcText = calcHelper.Calculation("÷");
            Assert.That(calcText == "0 ÷ ");
        }

        [Test]
        public void DigitsTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 + 2");
        }

        [Test]
        public void AddTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 + 2");

            calcText = calcHelper.Calculation("=");
            Assert.That(calcText == "3  ");
        }

        [Test]
        public void SubtractTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("-");
            Assert.That(calcText == "1 - ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 - 2");

            calcText = calcHelper.Calculation("=");
            Assert.That(calcText == "-1  ");
        }

        [Test]
        public void DevideTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("÷");
            Assert.That(calcText == "1 ÷ ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 ÷ 2");

            calcText = calcHelper.Calculation("=");
            Assert.That(calcText == "0.5  ");
        }

        [Test]
        public void MultiplyTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("*");
            Assert.That(calcText == "1 * ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 * 2");

            calcText = calcHelper.Calculation("=");
            Assert.That(calcText == "2  ");
        }

        [Test]
        public void DotTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation(".");
            Assert.That(calcText == "1.  ");

            calcText = calcHelper.Calculation(".");
            Assert.That(calcText == "1.  ");
        }

        [Test]
        public void SymbolFullDigitTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 + 2");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "3 + ");
        }

        [Test]
        public void DeleteDigitTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("DEL");
            Assert.That(calcText == "  ");

            calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("DEL");
            Assert.That(calcText == "  ");

            calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("DEL");
            Assert.That(calcText == "1  ");
        }

        [Test]
        public void ClearAllTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 + 2");

            calcText = calcHelper.Calculation("C");
            Assert.That(calcText == "  ");
        }

        [Test]
        public void ChangeSignTest()
        {
            CalculatorHelper calcHelper = new CalculatorHelper();
            var calcText = calcHelper.Calculation("1");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+/-");
            Assert.That(calcText == "-1  ");

            calcText = calcHelper.Calculation("+/-");
            Assert.That(calcText == "1  ");

            calcText = calcHelper.Calculation("+");
            Assert.That(calcText == "1 + ");

            calcText = calcHelper.Calculation("2");
            Assert.That(calcText == "1 + 2");

            calcText = calcHelper.Calculation("+/-");
            Assert.That(calcText == "1 + -2");

            calcText = calcHelper.Calculation("+/-");
            Assert.That(calcText == "1 + 2");

            calcText = calcHelper.Calculation("+/-");
            Assert.That(calcText == "1 + -2");

            calcText = calcHelper.Calculation("=");
            Assert.That(calcText == "-1  ");
        }
    }
}
