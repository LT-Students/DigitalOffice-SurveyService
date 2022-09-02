using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option.Create;

public record CreateOptionRequest : IRequest<Guid>
{
  public Guid QuestionId { get; set; }
  [Required]
  public string Content { get; set; }
  public bool IsCustom { get; set; }
}
