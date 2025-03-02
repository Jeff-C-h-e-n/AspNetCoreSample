using CoinDesk.Models;
using CoinDesk.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoinDesk.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetCurrency(string code)
        {
            var currency = await _currencyService.GetCurrencyByCodeAsync(code);
            if (currency == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(currency);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencies()
        {
            return Ok(await _currencyService.GetAllCurrenciesOrderByCodeAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency(Currency currency)
        {
            if (!await _currencyService.AddCurrencyAsync(currency))
            {
                return StatusCode((int)HttpStatusCode.Conflict, HttpStatusCode.Conflict.ToString());
            }
            else
            {
                return CreatedAtAction(nameof(GetCurrency), new { code = currency.Code }, currency);
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateCurrency(string code, Currency currency)
        {
            if (!await _currencyService.UpdateCurrencyNameByCodeAsync(code, currency))
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteCurrency(string code)
        {
            if (!await _currencyService.DeleteCurrencyByCodeAsync(code))
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }
}