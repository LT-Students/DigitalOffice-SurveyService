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

  public async Task<bool> CreateAsync(List<DbUserAnswer> dbUserAnswers)
  {
    if (dbUserAnswers is null)
    {
      return false;
    }

    _provider.UsersAnswers.AddRange(dbUserAnswers);
    await _provider.SaveAsync();

    return true;
  }
}
