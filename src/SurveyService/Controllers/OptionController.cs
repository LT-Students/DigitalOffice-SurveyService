using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[Route("[controller]")]
[ApiController]
public class OptionController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateOptionCommand command,
    [FromBody] CreateOptionRequest request)
  {
    return await command.ExecuteAsync(request);
  }

  [HttpPatch("edit")]
  public async Task<OperationResultResponse<bool>> EditAsync(
    [FromServices] IEditOptionCommand command,
    [FromQuery] Guid optionId,
    [FromBody] JsonPatchDocument<EditOptionRequest> request)
  {
    return await command.ExecuteAsync(optionId, request);
  }
}
