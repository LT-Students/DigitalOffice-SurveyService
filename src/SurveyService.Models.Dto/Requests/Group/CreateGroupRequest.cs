using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

public record CreateGroupRequest
{
  [Required(ErrorMessage = "Subject is required.")]
  public string Subject { get; set; }
  public string Description { get; set; }

  [Required(ErrorMessage = "At least 1 question is required.")]
  public ICollection<CreateQuestionRequest> Questions { get; set; }
}
