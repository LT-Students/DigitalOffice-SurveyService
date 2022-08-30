using LT.DigitalOffice.SurveyService.Models.Db;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

public record CreateGroupRequest
{
  public string Subject { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
  
  public ICollection<DbQuestion> Questions { get; set; }
}