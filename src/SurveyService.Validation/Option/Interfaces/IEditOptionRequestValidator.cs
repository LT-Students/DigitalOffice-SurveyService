using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

[AutoInject]
public interface IEditOptionRequestValidator : IValidator<JsonPatchDocument<EditOptionRequest>>
{
}
