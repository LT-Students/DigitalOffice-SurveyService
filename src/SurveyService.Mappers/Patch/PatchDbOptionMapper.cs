using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch;

public class PatchDbOptionMapper : IPatchDbOptionMapper
{
  public JsonPatchDocument<DbOption> Map(JsonPatchDocument<EditOptionRequest> request)
  {
    return null;
  }
}
