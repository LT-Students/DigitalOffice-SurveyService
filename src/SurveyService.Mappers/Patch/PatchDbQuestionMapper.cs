using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch;

public class PatchDbQuestionMapper : IPatchDbQuestionMapper
{
  public JsonPatchDocument<DbQuestion> Map(JsonPatchDocument<EditQuestionRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbQuestion> dbRequest = new JsonPatchDocument<DbQuestion>();

    foreach (Operation<EditQuestionRequest> item in request.Operations)
    {
      dbRequest.Operations.Add(new Operation<DbQuestion>(item.op, item.path, item.from, item.value));
    }

    return dbRequest;
  }
}
