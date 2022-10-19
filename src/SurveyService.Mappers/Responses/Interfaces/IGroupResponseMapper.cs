using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;

[AutoInject]
public interface IGroupResponseMapper
{
  GroupResponse Map(DbGroup dbGroup, List<UserData> usersData);
}
