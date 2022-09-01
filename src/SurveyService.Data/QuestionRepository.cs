using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
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

  public Task<DbQuestion> GetAsync(Guid questionId)
  {
    return _provider.Questions
      .FirstOrDefaultAsync(x => x.Id == questionId);
  }
}
