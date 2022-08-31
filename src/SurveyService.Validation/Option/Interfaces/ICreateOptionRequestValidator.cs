using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;

namespace LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

[AutoInject]
public interface ICreateOptionRequestValidator : IValidator<(CreateOptionRequest request, bool? hasCustomOptions)>
{
}
