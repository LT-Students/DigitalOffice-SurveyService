using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef.Models;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class OptionRepository : IOptionRepository
{
  private readonly IDataProvider _provider;

  public OptionRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<Guid?> CreateAsync(DbOption dbOption)
  {
    if (dbOption is null)
    {
      return null;
    }

    _provider.Options.Add(dbOption);
    await _provider.SaveAsync();

    return dbOption.Id;
  }
}
