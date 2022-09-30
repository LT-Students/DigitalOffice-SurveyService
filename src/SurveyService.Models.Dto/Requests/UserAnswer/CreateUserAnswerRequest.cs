using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;

public record CreateUserAnswerRequest
{
  [Required]
  public List<Guid> OptionIds { get; set; }
}
