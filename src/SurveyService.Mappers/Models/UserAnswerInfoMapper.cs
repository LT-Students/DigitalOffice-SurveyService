using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class UserAnswerInfoMapper : IUserAnswerInfoMapper
{
  public UserAnswerInfo Map(DbUserAnswer dbUserAnswer)
  {
    return dbUserAnswer is null
      ? null
      : new UserAnswerInfo
      {
        Id = dbUserAnswer.Id,
        UserId = dbUserAnswer.UserId
      };
  }
}
