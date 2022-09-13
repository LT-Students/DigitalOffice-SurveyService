﻿using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using Microsoft.EntityFrameworkCore;
using System;
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

  public async Task<DbQuestion> GetAsync(Guid id)
  {
    return null;
  }

  public async Task<DbQuestion> GetPropertiesAsync(GetQuestionPropertiesFilter filter)
  public Task<DbQuestion> GetAsync(Guid questionId)
  {
    DbQuestion question = new DbQuestion();

    if (filter.GroupId.HasValue)
    {
      question = await _provider.Questions.FirstOrDefaultAsync(q => q.Id == filter.GroupId);
    return _provider.Questions
      .FirstOrDefaultAsync(x => x.Id == questionId);
  }

    if (filter.QuestionId.HasValue)
  public Task<bool> DoesExistAsync(Guid questionId)
  {
      question = await _provider.Questions.FirstOrDefaultAsync(q => q.Id == filter.QuestionId);
    }

    return question;
    return _provider.Questions
      .AnyAsync(x => x.Id == questionId);
  }
}
