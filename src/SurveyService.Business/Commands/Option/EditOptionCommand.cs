using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option;

public class EditOptionCommand : IEditOptionCommand
{
  public Task<OperationResultResponse<bool>> ExecuteAsync(Guid optionId, JsonPatchDocument<EditOptionRequest> request)
  {
    return new Task<OperationResultResponse<bool>>(null);
  }
}
