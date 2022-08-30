using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto;

public record CreateOptionRequest
{
  public Guid QuestionId { get; set; }
  [Required]
  public string Content { get; set; }
  public bool IsCustom { get; set; }
}
