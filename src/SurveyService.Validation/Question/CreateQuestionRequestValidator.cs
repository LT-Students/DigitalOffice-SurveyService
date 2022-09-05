using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateQuestionRequestValidator : AbstractValidator<CreateQuestionRequest>, ICreateQuestionRequestValidator
{
  public CreateQuestionRequestValidator(
    IQuestionRepository _questionRepository
    )
  {
    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    RuleFor(q => q.Deadline)
      .Must(v => v > new DateTime(v.Value.Year, v.Value.Month, v.Value.Day + 1,
                                  v.Value.Hour, v.Value.Minute, v.Value.Millisecond))
      .WithMessage("The deadline is too close.");

    When(q => q.GroupId.HasValue, () =>
    {
      RuleFor(q => q)
        .Cascade(CascadeMode.Stop)
        .MustAsync(async (q, _) => (await _questionRepository.GetPropertiesAsync(new GetQuestionPropertiesFilter() { GroupId = q.GroupId })).Deadline == q.Deadline)
        .WithMessage("The deadlines of questions in the group are not equal.")
        .MustAsync(async (q, _) => (await _questionRepository.GetPropertiesAsync(new GetQuestionPropertiesFilter() { GroupId = q.GroupId })).HasRealTimeResult == q.HasRealTimeResults)
        .WithMessage("The conditions for displaying the results of the questions are different.");
    });

    When(q => !q.HasMultipleChoice && q.HasCustomOptions, () =>
    {
      RuleFor(q => q.Options)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("No options in the question.");
    });

    When(q => !q.HasCustomOptions, () =>
    {
      RuleForEach(q => q.Options)
        .Cascade(CascadeMode.Stop)
        .Must(q => !q.IsCustom)
        .WithMessage("This question can't have custom options.")
        .Must(q => q.Content.Length < 301);
    });
  }
}
