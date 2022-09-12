using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

public record CreateGroupQuestionRequest
{
  [Required]
  public string Content { get; set; }
  [Required]
  public bool IsAnonymous { get; set; }
  [Required]
  public bool IsRevoteAvaible { get; set; }
  [Required]
  public bool IsObligatory { get; set; }
  [Required]
  public bool IsPrivate { get; set; }
  [Required]
  public bool HasMultipleChoice { get; set; }
  [Required]
  public bool HasCustomOptions { get; set; }
  public List<CreateOptionRequest> Options { get; set; }
}