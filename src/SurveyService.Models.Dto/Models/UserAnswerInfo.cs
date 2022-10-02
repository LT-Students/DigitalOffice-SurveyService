using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Models;

public record UserAnswerInfo
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid OptionId { get; set; }
}
