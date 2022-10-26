using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Models;

public record QuestionInfo
{
  public Guid Id { get; set; }
  public string Content { get; set; }
  public DateTime? Deadline { get; set; }
  public bool HasRealTimeResult { get; set; }
  public bool IsAnonymous { get; set; }
  public bool IsRevoteAvailable { get; set; }
  public bool IsObligatory { get; set; }
  public bool IsPrivate { get; set; }
  public bool HasMultipleChoice { get; set; }
  public bool HasCustomOptions { get; set; }

  public List<OptionInfo> Options { get; set; }
}
