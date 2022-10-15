using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class GroupRepository : IGroupRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GroupRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Guid?> CreateAsync(DbGroup dbGroup)
  {
    if (dbGroup is null)
    {
      return null;
    }

    _provider.Groups.Add(dbGroup);
    await _provider.SaveAsync();
    
    return dbGroup.Id;
  }

  public async Task<(List<DbGroup>, int totalCount)> FindByAuthorAsync(FindByAuthorFilter filter)
  {
    IQueryable<DbGroup> query = _provider.Groups.AsQueryable();

    query = query.Where(g => g.CreatedBy == _httpContextAccessor.HttpContext.GetUserId());

    if (filter.IsAscendingSort.HasValue)
    {
      query = filter.IsAscendingSort.Value
        ? query.OrderBy(g => g)
        : query.OrderByDescending(g => g.Subject);
    }
    else
    {
      query = query.OrderByDescending(g => g.CreatedAtUtc);
    }

    if (filter.IsActive.HasValue)
    {
      query = filter.IsActive.Value
        ? query.Where(g => g.IsActive)
        : query.Where(g => !g.IsActive);
    }

    return (
      await query.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
      await query.CountAsync());
  }
}
