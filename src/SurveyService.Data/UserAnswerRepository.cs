using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;
public class UserAnswerRepository : IUserAnswerRepository
{
  private readonly IDataProvider _provider;

  public UserAnswerRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<bool> CreateAsync(List<DbUserAnswer> dbUserAnswer)
  {
    if (dbUserAnswer is null)
    {
      return false;
    }

    _provider.UsersAnswers.AddRange(dbUserAnswer);
    await _provider.SaveAsync();

    return true;
  }
}
