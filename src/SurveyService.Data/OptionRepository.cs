using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using Microsoft.EntityFrameworkCore;
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

  public Task<bool> DoesExistAsync(Guid optionId)
  {
    return _provider.Options
      .AnyAsync(o => o.Id == optionId);
  }
}
