using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbOptionMapper
{
  JsonPatchDocument<DbOption> Map(JsonPatchDocument<EditOptionRequest> request);
}