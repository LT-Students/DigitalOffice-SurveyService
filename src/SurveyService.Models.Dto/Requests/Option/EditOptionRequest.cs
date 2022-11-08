namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;

public record EditOptionRequest
{
  public string Content { get; set; }
  public bool IsCustom { get; set; }
}
