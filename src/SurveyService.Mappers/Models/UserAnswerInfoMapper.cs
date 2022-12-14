using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class UserAnswerInfoMapper : IUserAnswerInfoMapper
{
  private readonly IUserInfoMapper _userInfoMapper;
  
  public UserAnswerInfoMapper(
    IUserInfoMapper userInfoMapper)
  {
    _userInfoMapper = userInfoMapper;
  }
  
  public UserAnswerInfo Map(DbUserAnswer dbUserAnswer, List<UserData> usersData)
  {
    return dbUserAnswer is null
      ? null
      : new UserAnswerInfo
      {
        Id = dbUserAnswer.Id,
        User = _userInfoMapper.Map(usersData?.FirstOrDefault(x => x?.Id == dbUserAnswer.UserId))
      };
  }
}
