using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;

public record QuestionResponse
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
  public bool IsActive { get; set; }
  public List<OptionInfo> Options { get; set; }
}
