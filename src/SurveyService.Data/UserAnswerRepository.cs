using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;
public class UserAnswerRepository : IUserAnswerRepository
{
  private readonly IDataProvider _provider;

  public UserAnswerRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<Guid?> CreateAsync(DbUserAnswer dbUserAnswer)
  {
    if (dbUserAnswer is null)
    {
      return null;
    }

    _provider.UsersAnswers.Add(dbUserAnswer);
    await _provider.SaveAsync();

    return dbUserAnswer.Id;
  }
}
