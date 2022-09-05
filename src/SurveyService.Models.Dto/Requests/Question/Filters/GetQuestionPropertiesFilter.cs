using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;

public record GetQuestionPropertiesFilter
{
  public Guid? GroupId { get; set; }
  public Guid? QuestionId { get; set; }
}
