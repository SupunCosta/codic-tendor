using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetCurrencies(ApplicationDbContext context);
    Task<Currency> GetCurrencyById(ApplicationDbContext context, int id);
    Task<int> CreateCurrency(ApplicationDbContext context, CurrencyCreateDto currencyCreateDto);
    Task<int> UpdateCurrency(ApplicationDbContext context, Currency cur);
    Task<bool> DeleteCurrency(ApplicationDbContext context, int id);
}