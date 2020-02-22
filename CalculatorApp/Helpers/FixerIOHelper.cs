using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CalculatorApp.Models;
using Newtonsoft.Json.Linq;

namespace CalculatorApp.Helpers
{
    public class FixerIOHelper
    {
        private const string BaseUri = "http://data.fixer.io/api/";

        private string _apiKey;

        public FixerIOHelper(string apiKey)
        {
            ApiKey = apiKey;
        }

        private string ApiKey
        {
            get => !string.IsNullOrWhiteSpace(_apiKey) ? _apiKey : throw new InvalidOperationException("Fixer.io now requires an API key");
            set => _apiKey = value;
        }

        public double Convert(string from, string to, double amount)
        {
            return GetRate(from, to).Convert(amount);
        }

        public async Task<double> ConvertAsync(string from, string to, double amount)
        {
            return (await GetRateAsync(from, to)).Convert(amount);
        }

        public FixerRate Rate(string from, string to)
        {
            return GetRate(from, to);
        }

        public async Task<FixerRate> RateAsync(string from, string to)
        {
            return await GetRateAsync(from, to);
        }

        private FixerRate GetRate(string from, string to)
        {
            from = from.ToUpper();
            to = to.ToUpper();

            if (!Symbols.IsValid(from))
                throw new ArgumentException("Symbol not found for provided currency", "from");

            if (!Symbols.IsValid(to))
                throw new ArgumentException("Symbol not found for provided currency", "to");

            var url = GetFixerUrl();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                return ParseData(response.Content.ReadAsStringAsync().Result, from, to);
            }
        }

        private async Task<FixerRate> GetRateAsync(string from, string to)
        {
            from = from.ToUpper();
            to = to.ToUpper();

            if (!Symbols.IsValid(from))
                throw new ArgumentException("Symbol not found for provided currency", "from");

            if (!Symbols.IsValid(to))
                throw new ArgumentException("Symbol not found for provided currency", "to");

            var url = GetFixerUrl();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return ParseData(await response.Content.ReadAsStringAsync(), from, to);
            }
        }

        private FixerRate ParseData(string data, string from, string to)
        {
            var root = JObject.Parse(data);

            var rates = root.Value<JObject>("rates");
            var fromRate = rates.Value<double>(from);
            var toRate = rates.Value<double>(to);

            var rate = toRate / fromRate;

            var returnedDate = DateTime.ParseExact(root.Value<string>("date"), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);

            return new FixerRate(from, to, rate, returnedDate);
        }

        private string GetFixerUrl()
        {
            return $"{BaseUri}latest?access_key={ApiKey}";
        }
    }
}