using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

public record CreateGroupQuestionRequest
{
  [Required]
  public string Content { get; set; }
  public bool IsAnonymous { get; set; }
  public bool IsRevoteAvaible { get; set; }
  public bool IsObligatory { get; set; }
  public bool IsPrivate { get; set; }
  public bool HasMultipleChoice { get; set; }
  public bool HasCustomOptions { get; set; }
  public List<CreateQuestionOptionRequest> Options { get; set; }
}