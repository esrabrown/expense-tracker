using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Services;

namespace ExpenseTracker.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public IActionResult Index()
        {
            var currencies = new List<string> { "USD", "AUD", "CAD", "PLN", "MXN", "EUR" };
            ViewBag.Currencies = currencies;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetExchangeRates(string baseCurrency)
        {
            var targetCurrencies = new List<string> { "USD", "AUD", "CAD", "PLN", "MXN", "EUR" };
            var exchangeRates = new Dictionary<string, decimal>();

            try
            {
                foreach (var targetCurrency in targetCurrencies.Where(c => c != baseCurrency))
                {
                    var rate = await _currencyService.GetExchangeRate(baseCurrency, targetCurrency);
                    exchangeRates[targetCurrency] = rate;
                }

                ViewBag.BaseCurrency = baseCurrency;
                ViewBag.ExchangeRates = exchangeRates;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
            }

            ViewBag.Currencies = targetCurrencies;
            return View("Index");
        }
    }
}
