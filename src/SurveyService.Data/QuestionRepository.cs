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
    if (filter?.QuestionId is null)
    {
      return null;
    }
    
    IQueryable<DbQuestion> dbQuestions = _provider.Questions.AsQueryable();
    dbQuestions = dbQuestions.Where(question => question.Id == filter.QuestionId);
    if (filter.IncludeAnswers)
    {
      dbQuestions = filter.IncludeCustomOptions
        ? dbQuestions
          .Include(question => question.Options.Where(option => option.IsActive))
          .ThenInclude(option => option.UsersAnswers.Where(ua => filter.IncludeAnswers))
        : dbQuestions
          .Include(question => question.Options.Where(option => option.IsActive && !option.IsCustom))
          .ThenInclude(option => option.UsersAnswers.Where(ua => filter.IncludeAnswers));
    }
    else
    {
      dbQuestions = filter.IncludeCustomOptions
        ? dbQuestions
          .Include(question => question.Options.Where(option => option.IsActive))
        : dbQuestions
          .Include(question => question.Options.Where(option => option.IsActive && !option.IsCustom));
    }

    return dbQuestions.FirstOrDefaultAsync();
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
