using CalculatorApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// <param name="amount">The requested amount to Convert</param>
        /// Converts the requested currency amount into the new converted amount
        /// </summary>
        public double Convert(string from, string to, double amount)
        {
            return GetRate(from, to).Convert(amount);
        }

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// <param name="amount">The requested amount to Convert</param>
        /// Converts the requested currency amount into the new converted amount asynchronously
        /// </summary>
        public async Task<double> ConvertAsync(string from, string to, double amount)
        {
            return (await GetRateAsync(from, to)).Convert(amount);
        }

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// Gets the Currency difference rate
        /// </summary>
        public FixerRate Rate(string from, string to)
        {
            return GetRate(from, to);
        }

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// Gets the Currency difference rate asynchronously
        /// </summary>
        public async Task<FixerRate> RateAsync(string from, string to)
        {
            return await GetRateAsync(from, to);
        }

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// Main function the gets the Currency difference rate
        /// </summary>
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

        /// <summary>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// Main function the gets the Currency difference rate asynchronously
        /// </summary>
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

        /// <summary>
        /// <param name="data">The fixer.io response json</param>
        /// <param name="from">The requested currency to convert from</param>
        /// <param name="to">The requested currency to convert to</param>
        /// Parses the JSON response and creates a FixerRate object
        /// </summary>
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

        /// <summary>
        /// Returns the fixer.io full API Url
        /// </summary>
        private string GetFixerUrl()
        {
            return $"{BaseUri}latest?access_key={ApiKey}";
        }
    }
}