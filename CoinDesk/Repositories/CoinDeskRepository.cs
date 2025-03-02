using CoinDesk.DTOs;

namespace CoinDesk.Repositories
{
    public class CoinDeskRepository : ICoinDeskRepository
    {
        private readonly HttpClient _httpClient;

        public CoinDeskRepository(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("Logging");
        }

        public async Task<CoinDeskResponse?> GetBitcoinPriceAsync()
        {
            return await _httpClient.GetFromJsonAsync<CoinDeskResponse>("https://api.coindesk.com/v1/bpi/currentprice.json");
        }
    }
}