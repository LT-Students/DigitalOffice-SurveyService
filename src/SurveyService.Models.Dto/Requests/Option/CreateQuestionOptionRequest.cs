using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;

public record CreateQuestionOptionRequest
{
  [Required]
  public string Content { get; set; }
  public bool IsCustom { get; set; }
}
