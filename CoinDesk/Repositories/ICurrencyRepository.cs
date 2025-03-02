using CoinDesk.Models;

namespace CoinDesk.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetCurrencyAsync(string code);

        Task<IEnumerable<Currency>> GetCurrenciesAsync();

        Task AddCurrencyAsync(Currency currency);

        Task UpdateCurrencyNameAsync(Currency currency, string name);

        Task DeleteCurrencyAsync(Currency currency);
    }
}