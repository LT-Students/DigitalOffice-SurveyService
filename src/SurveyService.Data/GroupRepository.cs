using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
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
            .ThenInclude(question => question.Options.Where(option => option.IsActive && !option.IsCustom))
        : query
            .Include(group => group.Questions.Where(question => question.IsActive))
            .ThenInclude(question => question.Options.Where(option => option.IsActive))
            .ThenInclude(option => option.UsersAnswers);
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
            : CreateGetPredicates(filter, _provider.Groups.Where(group => group.Id == filter.GroupId).AsQueryable()).FirstOrDefaultAsync();
  }
}
