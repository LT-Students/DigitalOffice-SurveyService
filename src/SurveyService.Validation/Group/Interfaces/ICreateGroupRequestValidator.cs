using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;

namespace LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;

[AutoInject()]
public interface ICreateGroupRequestValidator : IValidator<CreateGroupRequest>
{
}
