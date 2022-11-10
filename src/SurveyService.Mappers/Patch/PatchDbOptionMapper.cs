using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch;

public class PatchDbOptionMapper : IPatchDbOptionMapper
{
  public JsonPatchDocument<DbOption> Map(JsonPatchDocument<EditOptionRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbOption> dbRequest = new();
    foreach (Operation<EditOptionRequest> requestOperation in request.Operations)
    {
      dbRequest.Operations.Add(new Operation<DbOption>(requestOperation.op, requestOperation.path,
        requestOperation.from, requestOperation.value));
    }

    return dbRequest;
  }
}
