using LT.DigitalOffice.SurveyService.Business.Commands.Option.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Controllers;

[ApiController, Route("[controller]")]
public class OptionController : ControllerBase
{
  private readonly IMediator _mediator;

  public OptionController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost("create")]
  public async Task<IActionResult> CreateAsync(
    [FromBody] CreateOptionRequest request,
    CancellationToken ct)
  {
    return Created(string.Empty, await _mediator.Send(request, ct));
  }
}
