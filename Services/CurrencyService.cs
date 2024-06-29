using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExpenseTracker.Services
{
    public class CurrencyService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "https://api.frankfurter.app/latest";

        public async Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency)
        {
            var url = $"{apiUrl}?amount=1&from={baseCurrency}&to={targetCurrency}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching exchange rates: {response.ReasonPhrase}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(responseString);

            decimal rate = data.rates[targetCurrency];

            return rate;
        }
    }
}
