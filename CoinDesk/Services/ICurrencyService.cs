using CoinDesk.Models;

namespace CoinDesk.Services
{
    public interface ICurrencyService
    {
        Task<Currency?> GetCurrencyByCodeAsync(string code);

        Task<IEnumerable<Currency>> GetAllCurrenciesOrderByCodeAsync();

        Task<bool> AddCurrencyAsync(Currency currency);

        Task<bool> UpdateCurrencyNameByCodeAsync(string code, Currency currency);

        Task<bool> DeleteCurrencyByCodeAsync(string code);
    }
}