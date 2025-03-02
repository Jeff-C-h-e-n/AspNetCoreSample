using CoinDesk.DTOs;

namespace CoinDesk.Services
{
    public interface ICoinDeskService
    {
        Task<RateResponse> GetBitcoinRatesAsync();
    }
}