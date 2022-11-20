using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;

[AutoInject]
public interface IEditGroupCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid GroupId, JsonPatchDocument<EditGroupRequest> request);
}
