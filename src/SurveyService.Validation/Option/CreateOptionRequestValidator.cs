using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

namespace LT.DigitalOffice.SurveyService.Validation.Option;

public class CreateOptionRequestValidator : AbstractValidator<(CreateOptionRequest request, bool? hasCustomOptions)>, ICreateOptionRequestValidator
{
  private readonly IQuestionRepository _questionRepository;

  public CreateOptionRequestValidator(
    IQuestionRepository questionRepository)
  {
    _questionRepository = questionRepository;

    When(x => !x.hasCustomOptions.HasValue, () =>
    {
      RuleFor(x => x.request.QuestionId)
        .MustAsync(async (x, _) => await _questionRepository.DoesExistAsync(x))
        .WithMessage("The question id doesn't exist.");

      When(x => x.request.IsCustom, () =>
      {
        RuleFor(x => x.request.QuestionId)
          .MustAsync(async (x, _) => (await _questionRepository.GetAsync(x)).HasCustomOptions)
          .WithMessage("This question can't have custom options.");
      });
    });

    When(x => x.hasCustomOptions.HasValue && x.request.IsCustom, () =>
    {
      RuleFor(x => x.hasCustomOptions)
        .Must(x => (bool)x)
        .WithMessage("This question can't have custom options.");
    });

    RuleFor(x => x.request.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");
  }
}
