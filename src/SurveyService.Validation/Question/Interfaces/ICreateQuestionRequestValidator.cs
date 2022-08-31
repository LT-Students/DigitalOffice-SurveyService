using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

namespace LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;

[AutoInject()]
public interface ICreateQuestionRequestValidator : IValidator<CreateQuestionRequest>
{
  
}