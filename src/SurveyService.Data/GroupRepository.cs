using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class GroupRepository : IGroupRepository
{
  private readonly IDataProvider _provider;

  public GroupRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  private IQueryable<DbGroup> CreateGetPredicates(
      GetGroupFilter filter,
      IQueryable<DbGroup> query)
  {
    if (filter.IncludeQuestions)
    {
      if (filter.IncludeOptions)
      {
        return filter.IncludeUserAnswers
        ? query
            .Include(group => group.Questions.Where(question => question.IsActive))
            .ThenInclude(question => question.Options.Where(option => option.IsActive))
            .ThenInclude(option => option.UsersAnswers)
        : query
            .Include(group => group.Questions.Where(question => question.IsActive))
            .ThenInclude(question => question.Options.Where(option => option.IsActive && !option.IsCustom));
      }

      return query
        .Include(group => group.Questions.Where(question => question.IsActive));
    }

    return query;
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

  public Task<DbGroup> GetAsync(GetGroupFilter filter)
  {
    return filter is null
            ? null
            : CreateGetPredicates(filter, _provider.Groups.Where(group => group.Id == filter.GroupId).AsQueryable())
              .FirstOrDefaultAsync();
  }

  public Task<DbGroup> GetAsync(Guid groupId)
  {
    return _provider.Groups.Where(group => group.Id == groupId)
      .Include(group => group.Questions)
      .ThenInclude(question => question.Options)
      .AsNoTracking()
      .FirstOrDefaultAsync();
  }

  public async Task<(List<DbGroup>, int totalCount)> FindByAuthorAsync(FindQuestionsFilter filter, Guid authorId)
  {
    IQueryable<DbGroup> query = _provider.Groups.AsQueryable().OrderByDescending(g => g.CreatedAtUtc);

    query = query.Where(g => g.CreatedBy == authorId);

    if (filter.IsAscendingSort.HasValue)
    {
      query = filter.IsAscendingSort.Value
        ? query.OrderBy(g => g)
        : query.OrderByDescending(g => g.Subject);
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

  public async Task<bool> EditAsync(JsonPatchDocument<DbGroup> patch, DbGroup dbGroup, Guid modifiedBy)
  {
    if (dbGroup is null || patch is null)
    {
      return false;
    }

    patch.ApplyTo(dbGroup);
    dbGroup.ModifiedBy = modifiedBy;
    dbGroup.ModifiedAtUtc = DateTime.UtcNow;

    await _provider.SaveAsync();

    return true;
  }
}
