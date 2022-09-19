using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateGroupQuestionRequestValidator : AbstractValidator<CreateGroupQuestionRequest>, ICreateGroupQuestionRequestValidator
{
  public CreateGroupQuestionRequestValidator()
  {
    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    When(q => !q.HasCustomOptions, () =>
    {
      RuleFor(q => q.Options)
        .NotEmpty()
        .WithMessage("This question should have one option at least.");
    });

    RuleForEach(q => q.Options)
      .Must(o => !o.IsCustom)
      .WithMessage("Option cannot be a custom.")
      .Must(o => o.Content.Length < 301)
      .WithMessage("Option is too long.");
  }
}
