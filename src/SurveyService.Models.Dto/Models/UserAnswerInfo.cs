using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Models;

public record UserAnswerInfo
{
  public Guid Id { get; set; }
  public UserInfo User { get; set; }
}
