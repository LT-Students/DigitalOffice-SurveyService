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
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Content can't be empty.")
      .MinimumLength(1).WithMessage("Content is too short.")
      .MaximumLength(300).WithMessage("Content is too long.");

    RuleFor(option => option)
      .MustAsync(async (o, _) =>
        (await _questionRepository.GetAsync(o.QuestionId))?.HasCustomOptions == o.IsCustom)
      .WithMessage("Wrong IsCustom value.");
  }
}
