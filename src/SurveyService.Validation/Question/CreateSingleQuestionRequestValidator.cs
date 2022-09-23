using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateSingleQuestionRequestValidator : AbstractValidator<CreateSingleQuestionRequest>, ICreateSingleQuestionRequestValidator
{
  public CreateSingleQuestionRequestValidator(
    IQuestionRepository questionRepository
    )
  {
    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    When(q => q.Deadline.HasValue, () =>
    {
      RuleFor(q => q.Deadline.Value)
        .Must(d => d > DateTime.UtcNow.AddDays(1).AddSeconds(2))
        .WithMessage("The deadline must be at least 24 hours from now.");
    }).Otherwise(() =>
    {
      RuleFor(question => question.HasRealTimeResult)
        .Must(realTime => realTime);
    });

    When(q => q.GroupId.HasValue, () =>
    {
      RuleFor(q => q)
        .MustAsync(async (q, _) => await questionRepository.CheckGroupProperties(q.GroupId.Value, q.Deadline, q.HasRealTimeResult))
        .WithMessage("Group properties are incorrect, please - check the deadline and result display settings");
    });

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
