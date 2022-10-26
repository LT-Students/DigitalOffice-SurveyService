using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;

public record GetGroupFilter
{
  [FromQuery(Name = "groupid")]
  public Guid GroupId { get; set; }

  [FromQuery(Name = "includequestions")]
  public bool IncludeQuestions { get; set; } = true;

  [FromQuery(Name = "includeoptions")]
  public bool IncludeOptions { get; set; } = true;

  [FromQuery(Name = "includeuseranswers")]
  public bool IncludeUserAnswers { get; set; } = false;

  [FromQuery(Name = "includeuserinfo")]
  public bool IncludeUserInfo { get; set; } = false;
}
