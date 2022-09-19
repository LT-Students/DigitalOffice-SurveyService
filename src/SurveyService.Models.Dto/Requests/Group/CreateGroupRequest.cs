using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

public record CreateGroupRequest
{
  [Required]
  public string Subject { get; set; }
  public string Description { get; set; }
  public DateTime? Deadline { get; set; }
  public bool HasRealTimeResults { get; set; }
  [Required]
  public List<CreateGroupQuestionRequest> Questions { get; set; }
}
