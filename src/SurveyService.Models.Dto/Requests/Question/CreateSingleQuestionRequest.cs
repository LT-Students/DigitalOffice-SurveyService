using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

public record CreateSingleQuestionRequest : CreateGroupQuestionRequest
{
  public Guid? GroupId { get; set; }
  public DateTime? Deadline { get; set; }
  public bool HasRealTimeResult { get; set; } = true;
}