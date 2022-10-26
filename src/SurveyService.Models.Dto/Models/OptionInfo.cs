using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Models;

public record OptionInfo
{
  public Guid Id { get; set; }
  public string Content { get; set; }
  public bool IsCustom { get; set; }
  public List<UserAnswerInfo> UsersAnswers { get; set; }
}
