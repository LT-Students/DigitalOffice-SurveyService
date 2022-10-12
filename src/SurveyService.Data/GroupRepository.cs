using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class GroupRepository : IGroupRepository
{
  private readonly IDataProvider _provider;

  public GroupRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  private IQueryable<DbGroup> CreateGetPredicates(
      GetGroupFilter filter,
      IQueryable<DbGroup> query)
  {
    if (filter.IncludeUserAnswers)
    {
      return query
              .Include(group => group.Questions.Where(question => question.IsActive))
              .ThenInclude(question => question.Options.Where(option => option.IsActive))
              .ThenInclude(option => option.UsersAnswers);
    }

    if (filter.IncludeOptions)
    {
      return query
              .Include(group => group.Questions.Where(question => question.IsActive))
              .ThenInclude(question => question.Options.Where(option => option.IsActive && !option.IsCustom));
    }

    if (filter.IncludeQuestions)
    {
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
    if (filter is null)
    {
      return null;
    }

    IQueryable<DbGroup> dbGroup = _provider
      .Groups
      .Where(group => group.Id == filter.GroupId)
      .AsQueryable();

    dbGroup = CreateGetPredicates(filter, dbGroup);

    return dbGroup.FirstOrDefaultAsync();
  }
}
