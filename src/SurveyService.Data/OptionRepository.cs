using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
  
  public Task<DbOption> GetAsync(Guid optionId)
  {
    return _provider.Options.FirstOrDefaultAsync(x => x.Id == optionId);
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

  public Task DeactivateAsync(ICollection<DbOption> dbOptions, Guid modifiedBy)
  {
    foreach (DbOption dbOption in dbOptions)
    {
      dbOption.IsActive = false;
      dbOption.ModifiedAtUtc = DateTime.UtcNow;
      dbOption.ModifiedBy = modifiedBy;
    }

    return _provider.SaveAsync();
  }

  public async Task<bool> EditAsync(JsonPatchDocument<DbOption> patch, DbOption dbOption, Guid? modifiedBy = null)
  {
    if (patch is null || dbOption is null)
    {
      return false;
    }
    
    patch.ApplyTo(dbOption);
    dbOption.ModifiedBy = modifiedBy ?? _httpContextAccessor.HttpContext.GetUserId();
    dbOption.ModifiedAtUtc = DateTime.UtcNow;
    await _provider.SaveAsync();

    return true;
  }
}
