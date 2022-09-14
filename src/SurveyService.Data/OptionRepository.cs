using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

  public async Task<List<DbOption>> GetByIdsAsync(List<Guid> optionIds)
  {
    return await _provider.Options
      .Where(dbOption => optionIds.Contains(dbOption.Id))
      .Include(option => option.Question)
      .Include(option => option.UsersAnswers)
      .ToListAsync();
  }
}
