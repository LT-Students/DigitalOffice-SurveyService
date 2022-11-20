namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

public record EditGroupRequest
{
  public string Subject { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
}
