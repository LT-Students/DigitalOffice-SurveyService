using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbQuestionMapper
{
  JsonPatchDocument<DbQuestion> Map(JsonPatchDocument<EditQuestionRequest> request);
}
