using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.SurveyService.Mappers.Patch;

public class PatchDbGroupMapper : IPatchDbGroupMapper
{
  public JsonPatchDocument<DbGroup> Map(JsonPatchDocument<EditGroupRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbGroup> dbRequest = new();

    foreach (Operation<EditGroupRequest> item in request.Operations)
    {
      dbRequest.Operations.Add(new Operation<DbGroup>(item.op, item.path, item.from, item.value));
    }

    return dbRequest;
  }
}