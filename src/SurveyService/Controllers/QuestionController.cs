using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
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

  [HttpGet("find")]
  public async Task<FindResultResponse<FindQuestionsResultInfo>> FindAsync(
    [FromServices] IFindQuestionsCommand command,
    [FromQuery] FindQuestionsFilter filter)
  {
    return await command.ExecuteAsync(filter);
  }
}