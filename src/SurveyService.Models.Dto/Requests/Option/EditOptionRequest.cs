using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;

public record EditOptionRequest
{
  public string Content { get; set; }
  public bool IsActive { get; set; }
}
