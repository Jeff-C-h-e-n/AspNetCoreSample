using CoinDesk.Data;
using CoinDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Currency?> GetCurrencyAsync(string code)
        {
            return await _context.Currencies.FindAsync(code);
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCurrencyNameAsync(Currency currency, string name)
        {
            currency.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCurrencyAsync(Currency currency)
        {
            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
        }
    }
}