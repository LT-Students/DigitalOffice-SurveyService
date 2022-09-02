using System;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;

public record CreateUserAnswerRequest
{ 
  public Guid OptionId { get; set; }
}
