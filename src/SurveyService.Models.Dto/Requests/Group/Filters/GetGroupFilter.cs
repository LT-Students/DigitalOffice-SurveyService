using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;

public record GetGroupFilter
{
  [Required]
  [FromQuery(Name = "groupid")]
  public Guid GroupId { get; set; }

  [FromQuery(Name = "includeQuestions")]
  public bool IncludeQuestions { get; set; } = true;

  [FromQuery(Name = "includeOptions")]
  public bool IncludeOptions { get; set; } = true;

  [FromQuery(Name = "includeUserAnswers")]
  public bool IncludeUserAnswers { get; set; } = false;

  [FromQuery(Name = "includeuserinfo")]
  public bool IncludeUserInfo { get; set; } = false;
}