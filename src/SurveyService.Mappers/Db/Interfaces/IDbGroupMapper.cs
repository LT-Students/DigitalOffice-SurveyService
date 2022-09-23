using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbGroupMapper
{
  DbGroup Map(CreateGroupRequest createGroupRequest);
}
