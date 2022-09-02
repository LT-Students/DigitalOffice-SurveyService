using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;

namespace LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;

[AutoInject]
public interface ICreateUserAnswerRequestValidator : IValidator<CreateUserAnswerRequest>
{
}
