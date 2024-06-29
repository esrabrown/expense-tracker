using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExpenseTracker.Services
{
    public class CurrencyService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "450670e6ca4ca1f1ea5bec32ea1a9399";
        private const string apiUrl = "http://api.exchangeratesapi.io/v1/latest";


        public async Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency)
        {
            var url = $"{apiUrl}?access_key={apiKey}&base={baseCurrency}&symbols={targetCurrency}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching exchange rates: {response.ReasonPhrase}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(responseString);

            if (data.rates == null || data.rates[targetCurrency] == null)
            {
                throw new HttpRequestException($"Error fetching exchange rates: Invalid response format.");
            }

            return data.rates[targetCurrency];
        }
    }
}

