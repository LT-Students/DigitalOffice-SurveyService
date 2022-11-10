using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;

[AutoInject]
public interface IEditOptionCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid optionId, JsonPatchDocument<EditOptionRequest> patch);
}
