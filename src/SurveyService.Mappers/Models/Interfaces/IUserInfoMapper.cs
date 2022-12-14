using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;

namespace LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;

[AutoInject]
public interface IUserInfoMapper
{
  UserInfo Map(UserData userData);
}
