using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[Route("[controller]")]
[ApiController]
public class UserAnswerController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<bool>> CreateAsync(
    [FromServices] ICreateUserAnswerCommand command,
    [FromBody] CreateUserAnswerRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}
