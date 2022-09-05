using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

namespace LT.DigitalOffice.SurveyService.Validation.Option;

public class CreateOptionRequestValidator : AbstractValidator<CreateOptionRequest>, ICreateOptionRequestValidator
{
  public CreateOptionRequestValidator(
    IQuestionRepository _questionRepository)
  {
    RuleFor(x => x.QuestionId)
      .MustAsync(async (x, _) => await _questionRepository.DoesExistAsync(x))
      .WithMessage("The question id doesn't exist.");

    WhenAsync(async (x, _) => await _questionRepository.DoesExistAsync(x.QuestionId) && x.IsCustom, () =>
    {
      RuleFor(x => x.QuestionId)
        .MustAsync(async (x, _) => (await _questionRepository.GetAsync(x)).HasCustomOptions)
        .WithMessage("This question can't have custom options.");
    });

    RuleFor(x => x.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");
  }
}