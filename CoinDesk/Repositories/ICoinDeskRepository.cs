using CoinDesk.DTOs;

namespace CoinDesk.Repositories
{
    public interface ICoinDeskRepository
    {
        Task<CoinDeskResponse?> GetBitcoinPriceAsync();
    }
}