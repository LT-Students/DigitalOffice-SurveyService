using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;

public class GetQuestionFilter
{
  [FromQuery(Name = "questionid")]
  public Guid? QuestionId { get; set; }

  [FromQuery(Name = "includecustomoptions")]
  public bool IncludeCustomOptions { get; set; } = true;
  
  [FromQuery(Name = "includeanswers")]
  public bool IncludeAnswers { get; set; }
  
  [FromQuery(Name = "includeuserinfo")]
  public bool IncludeUserInfo { get; set; }
}
