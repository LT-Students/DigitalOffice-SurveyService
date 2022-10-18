using LT.DigitalOffice.SurveyService.Models.Dto.Enums;
using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Models;

public record FindQuestionsResultInfo
{
  public ItemType ItemType { get; set; }
  public Guid ItemId { get; set; }
  public string Content { get; set; }
  public bool IsActive { get; set; }
}
