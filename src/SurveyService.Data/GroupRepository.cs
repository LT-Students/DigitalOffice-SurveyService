using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class GroupRepository : IGroupRepository
{
  private readonly IDataProvider _provider;

  public GroupRepository(IDataProvider provider)
  {
    _provider = provider;
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

    IQueryable<DbGroup> dbGroup = _provider.Groups.AsQueryable();
    dbGroup = dbGroup.Where(group => group.Id == filter.GroupId && group.IsActive);

    if (filter.IncludeUserAnswers)
    {
      return dbGroup
              .Include(group => group.Questions.Where(question => question.IsActive))
              .ThenInclude(question => question.Options.Where(option => option.IsActive))
              .ThenInclude(option => option.UsersAnswers)
              .FirstOrDefaultAsync();
    }

    if (filter.IncludeOptions)
    {
      return dbGroup
              .Include(group => group.Questions.Where(question => question.IsActive))
              .ThenInclude(question => question.Options.Where(option => option.IsActive && !option.IsCustom))
              .FirstOrDefaultAsync();
    }

    if (filter.IncludeQuestions)
    {
      return dbGroup
              .Include(group => group.Questions.Where(question => question.IsActive))
              .FirstOrDefaultAsync();
    }

    return dbGroup.FirstOrDefaultAsync();
  }
}
