using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

public class EditQuestionRequest
{
  public string Content { get; set; }
  public bool IsAnonymous { get; set; }
  public bool IsRevoteAvailable { get; set; }
  public bool IsObligatory { get; set; }
  public bool IsPrivate { get; set; }
  public bool HasMultipleChoice { get; set; }
  public bool IsActive { get; set; }
  public DateTime? Deadline { get; set; }
  public bool HasRealTimeResult { get; set; }
}
