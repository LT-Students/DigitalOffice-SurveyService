using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;

[AutoInject]
public interface ICreateUserAnswerRequestValidator : IValidator<(CreateUserAnswerRequest, List<DbOption>)>
{
}
