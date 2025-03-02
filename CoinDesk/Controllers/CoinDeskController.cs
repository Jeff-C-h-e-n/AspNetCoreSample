using CoinDesk.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.Controllers
{
    [ApiController]
    [Route("api/rates")]
    public class CoinDeskController : ControllerBase
    {
        private readonly ICoinDeskService _coinDeskService;

        public CoinDeskController(ICoinDeskService coinDeskService)
        {
            _coinDeskService = coinDeskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBitcoinRates(bool demoException = false)
        {
            if (demoException)
            {
                throw new Exception("Demo exception");
            }

            return Ok(await _coinDeskService.GetBitcoinRatesAsync());
        }
    }
}