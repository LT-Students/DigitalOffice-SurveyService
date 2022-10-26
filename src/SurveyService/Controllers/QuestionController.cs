using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[Route("[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateQuestionCommand command,
    [FromBody] CreateSingleQuestionRequest request)
  {
    return await command.ExecuteAsync(request);
  }

  [HttpPatch("edit")]
  public async Task<OperationResultResponse<bool>> EditAsync(
    [FromServices] IEditQuestionCommand command,
    [FromQuery] Guid questionId,
    [FromBody] JsonPatchDocument<EditQuestionRequest> request)
  {
    return await command.ExecuteAsync(questionId, request);
  }
}
