using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
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
  
  [HttpGet("get")]
  public async Task<OperationResultResponse<QuestionResponse>> CreateAsync(
    [FromServices] IGetQuestionCommand command,
    [FromQuery] Guid questionId)
  {
    return await command.ExecuteAsync(questionId);
  }
}
