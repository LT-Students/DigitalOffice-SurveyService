using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Group;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[Route("[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateGroupCommand command,
    [FromBody] CreateGroupRequest request)
  {
    return await command.ExecuteAsync(request);
  }

  [HttpGet("get")]
  public async Task<OperationResultResponse<GroupResponse>> GetAsync(
    [FromServices] IGetGroupCommand command,
    [FromQuery] GetGroupFilter filter)
  {
    return await command.ExecuteAsync(filter);
  }

  [HttpPatch("edit")]
  public async Task<OperationResultResponse<bool>> EditAsync(
    [FromServices] IEditGroupCommand command,
    [FromQuery] Guid groupId,
    [FromBody] JsonPatchDocument<EditGroupRequest> request)
  {
    return await command.ExecuteAsync(groupId, request);
  }
}
