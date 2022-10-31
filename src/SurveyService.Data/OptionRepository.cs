using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class OptionRepository : IOptionRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public OptionRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
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

  public async Task<List<DbOption>> GetByIdsAsync(List<Guid> optionIds)
  {
    return await _provider.Options
      .Where(dbOption => optionIds.Contains(dbOption.Id))
      .Include(option => option.Question)
        .ThenInclude(question => question.Options)
          .ThenInclude(option => option.UsersAnswers)
      .Include(option => option.Question.Group)
        .ThenInclude(group => group.Questions)
      .ToListAsync();
  }

  public Task DisactivateAsync(ICollection<DbOption> options)
  {
    foreach (DbOption dbOption in options)
    {
      dbOption.IsActive = false;
      dbOption.ModifiedAtUtc = DateTime.UtcNow;
      dbOption.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
    }

    return _provider.SaveAsync();
  }
}
