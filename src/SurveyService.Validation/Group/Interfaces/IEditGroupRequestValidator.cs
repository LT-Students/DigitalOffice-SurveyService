using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;

[AutoInject]
public interface IEditGroupRequestValidator : IValidator<JsonPatchDocument<EditGroupRequest>>
{
}
