using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class UserInfoMapper : IUserInfoMapper
{
  public UserInfo Map(UserData userData)
  {
    return userData is null
      ? null
      : new UserInfo 
      { 
        Id = userData.Id,
        FirstName = userData.FirstName,
        MiddleName = userData.MiddleName,
        LastName = userData.LastName
      };
  }
}
