using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto;

namespace LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

[AutoInject]
public interface ICreateOptionRequestValidator : IValidator<CreateOptionRequest>
{
}
