using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbGroupMapper
{
  JsonPatchDocument<DbGroup> Map(JsonPatchDocument<EditGroupRequest> request);
}

