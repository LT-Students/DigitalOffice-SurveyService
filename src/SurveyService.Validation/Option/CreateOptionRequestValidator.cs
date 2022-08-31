using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;

namespace LT.DigitalOffice.SurveyService.Validation.Option;

public class CreateOptionRequestValidator : AbstractValidator<CreateOptionRequest>, ICreateOptionRequestValidator
{
  private readonly IQuestionRepository _questionRepository;

  public CreateOptionRequestValidator(
    IQuestionRepository questionRepository)
  {
    _questionRepository = questionRepository;

    RuleFor(option => option.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    When(option => option.IsCustom, () =>
    {
      RuleFor(option => option.QuestionId)
      .Cascade(CascadeMode.Stop)
      .MustAsync(async (option, _) => await _questionRepository.GetAsync(option) is not null)
      .WithMessage("Can't create option for non-existent question.")
      .MustAsync(async (option, _) => await _questionRepository.GetAsync(option).HasCustomOptions)
      .WithMessage("This question can't have custom options.");
    });
  }
}
