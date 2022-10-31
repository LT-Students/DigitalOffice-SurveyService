using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class QuestionRepository : IQuestionRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public QuestionRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
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

  public async Task<bool> CheckGroupProperties(Guid groupId, DateTime? deadline, bool hasRealTimeResult)
  {
    DbQuestion question = await _provider.Questions.FirstOrDefaultAsync(q => q.GroupId == groupId);

    if ((question is null) || (question.Deadline != deadline) || (question.HasRealTimeResult != hasRealTimeResult))
    {
      return false;
    }

    return true;
  }

  public Task<bool> DoesExistAsync(Guid questionId)
  {
    return _provider.Questions.AnyAsync(x => x.Id == questionId);
  }

  public async Task<(List<DbQuestion>, int totalCount)> FindByAuthorAsync(FindQuestionsFilter filter, Guid authorId)
  {
    IQueryable<DbQuestion> query = _provider.Questions.AsQueryable().OrderByDescending(q => q.CreatedAtUtc);

    query = query.Where(q => q.CreatedBy == authorId && q.GroupId == null);

    if (filter.IsAscendingSort.HasValue)
    {
      query = filter.IsAscendingSort.Value
        ? query.OrderBy(q => q.Content)
        : query.OrderByDescending(q => q.Content);
    }

    if (filter.IsActive.HasValue)
    {
      query = filter.IsActive.Value
        ? query.Where(q => q.IsActive)
        : query.Where(q => !q.IsActive);
    }

    return (
      await query.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
      await query.CountAsync());
  }

  public async Task<bool> EditAsync(JsonPatchDocument<DbQuestion> patch, DbQuestion question)
  {

    if (question is null || patch is null)
    {
      return false;
    }

    patch.ApplyTo(question);
    question.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
    question.ModifiedAtUtc = DateTime.UtcNow;

    await _provider.SaveAsync();

    return true;
  }

  public async Task<DbQuestion> GetQuestionWithAnswersAsync(Guid questionId)
  {
    return await _provider.Questions
      .Where(q => q.Id == questionId)
      .Include(question => question.Options)
      .ThenInclude(option => option.UsersAnswers)
      .FirstOrDefaultAsync();
  }
}
