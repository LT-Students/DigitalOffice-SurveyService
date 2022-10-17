using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class QuestionRepository : IQuestionRepository
{
  private readonly IDataProvider _provider;

  private IQueryable<DbQuestion> CreateGetPredicates(
    GetQuestionFilter filter,
    IQueryable<DbQuestion> dbQuestions)
  { 
    if (filter.IncludeAnswers)
    {
      dbQuestions = dbQuestions
        .Include(question => question.Options.Where(option => option.IsActive))
          .ThenInclude(option => option.UsersAnswers);
    }
    else
    {
      dbQuestions = dbQuestions
        .Include(question => question.Options.Where(option => option.IsActive));
    }

    return dbQuestions;
  }
  
  public QuestionRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<Guid?> CreateAsync(DbQuestion dbQuestion)
  {
    if (dbQuestion is null)
    {
      return null;
    }

    _provider.Questions.Add(dbQuestion);
    await _provider.SaveAsync();

    return dbQuestion.Id;
  }

  public Task<DbQuestion> GetAsync(Guid questionId)
  {
    return _provider.Questions.FirstOrDefaultAsync(x => x.Id == questionId);
  }

  public Task<DbQuestion> GetAsync(GetQuestionFilter filter)
  {
    return filter is null
      ? Task.FromResult(default(DbQuestion))
      : CreateGetPredicates(filter,
        _provider.Questions.AsQueryable().Where(question => question.Id == filter.QuestionId))
        .FirstOrDefaultAsync();
  }

  public async Task<bool> CheckGroupProperties(Guid groupId, DateTime? deadline, bool hasRealTimeResult)
  {
    DbQuestion question = await _provider.Questions.FirstOrDefaultAsync(q => q.GroupId == groupId);

    if((question is null) || (question.Deadline != deadline) || (question.HasRealTimeResult != hasRealTimeResult))
    {
      return false;
    }

    return true;
  }

  public Task<bool> DoesExistAsync(Guid questionId)
  {
    return _provider.Questions.AnyAsync(x => x.Id == questionId);
  }
}
