using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;

public record RemoveUserAnswerRequest
{
  [Required]
  public Guid questionId { get; set; }

  [Required]
  public Guid userAnswerId { get; set; }
}
