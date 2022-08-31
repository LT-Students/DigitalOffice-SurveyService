using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[Route("[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid>> CreateAsync(
    ICreateGroupCommand command,
    [FromBody] CreateGroupRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}