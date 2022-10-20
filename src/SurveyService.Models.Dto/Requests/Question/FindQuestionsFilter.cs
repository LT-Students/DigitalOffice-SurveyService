using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

public record FindQuestionsFilter : BaseFindFilter
{
  [FromQuery(Name = "isascendingsort")]
  public bool? IsAscendingSort { get; set; }

  [FromQuery(Name = "isactive")]
  public bool? IsActive { get; set; } = true;

  [FromQuery(Name = "includegroup")]
  public bool IncludeGroup { get; set; } = true;
}
