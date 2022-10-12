using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;

public class GetQuestionFilter
{
  [Required]
  [FromQuery(Name = "questionid")]
  public Guid QuestionId { get; set; }

  [FromQuery(Name = "includecustomoptions")]
  public bool IncludeCustomOptions { get; set; } = true;

  [FromQuery(Name = "includeanswers")]
  public bool IncludeAnswers { get; set; } = false;

  [FromQuery(Name = "includeuserinfo")]
  public bool IncludeUserInfo { get; set; } = false;
}
