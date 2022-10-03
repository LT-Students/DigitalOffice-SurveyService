using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class UserAnswerInfoMapper : IUserAnswerInfoMapper
{
  private readonly IUserInfoMapper _userInfoMapper;
  
  public UserAnswerInfoMapper(
    IUserInfoMapper userInfoMapper)
  {
    _userInfoMapper = userInfoMapper;
  }
  
  public UserAnswerInfo Map(DbUserAnswer dbUserAnswer, UserData userData)
  {
    return dbUserAnswer is null
      ? null
      : new UserAnswerInfo
      {
        Id = dbUserAnswer.Id,
        UserId = dbUserAnswer.UserId,
        User = _userInfoMapper.Map(userData)
      };
  }
}
