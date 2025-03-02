using CoinDesk.Models;
using CoinDesk.Repositories;
using CoinDesk.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency?> GetCurrencyByCodeAsync(string code)
    {
        return await _currencyRepository.GetCurrencyAsync(code);
    }

    public async Task<IEnumerable<Currency>> GetAllCurrenciesOrderByCodeAsync()
    {
        var currencies = await _currencyRepository.GetCurrenciesAsync();
        return currencies.OrderBy(c => c.Code);
    }

    public async Task<bool> AddCurrencyAsync(Currency currency)
    {
        if (await _currencyRepository.GetCurrencyAsync(currency.Code) != null)
        {
            return false;
        }
        else
        {
            await _currencyRepository.AddCurrencyAsync(currency);
            return true;
        }
    }

    public async Task<bool> UpdateCurrencyNameByCodeAsync(string code, Currency currency)
    {
        var existingCurrency = await _currencyRepository.GetCurrencyAsync(code);
        if (existingCurrency == null)
        {
            return false;
        }
        else
        {
            await _currencyRepository.UpdateCurrencyNameAsync(existingCurrency, currency.Name);
            return true;
        }
    }

    public async Task<bool> DeleteCurrencyByCodeAsync(string code)
    {
        var currency = await _currencyRepository.GetCurrencyAsync(code);
        if (currency == null)
        {
            return false;
        }
        else
        {
            await _currencyRepository.DeleteCurrencyAsync(currency);
            return true;
        }
    }
}