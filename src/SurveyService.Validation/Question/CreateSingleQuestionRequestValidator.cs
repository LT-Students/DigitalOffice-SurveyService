using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Resources;
using System;
using System.Globalization;
using System.Threading;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateSingleQuestionRequestValidator : AbstractValidator<CreateSingleQuestionRequest>, ICreateSingleQuestionRequestValidator
{
  public CreateSingleQuestionRequestValidator(
    IQuestionRepository questionRepository
    )
  {
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");

    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage(CreateQuestionRequestValidatorResource.ContentLong);

    When(q => q.Deadline.HasValue, () =>
    {
      RuleFor(q => q.Deadline.Value)
        .Must(d => d > DateTime.UtcNow.AddDays(1).AddSeconds(2))
        .WithMessage(CreateQuestionRequestValidatorResource.Deadline);
    }).Otherwise(() =>
    {
      RuleFor(question => question.HasRealTimeResult)
        .Must(realTime => realTime);
    });

    When(q => q.GroupId.HasValue, () =>
    {
      RuleFor(q => q)
        .MustAsync(async (q, _) => await questionRepository.CheckGroupProperties(q.GroupId.Value, q.Deadline, q.HasRealTimeResult))
        .WithMessage(CreateQuestionRequestValidatorResource.GroupIncorrect);
    });

    When(q => !q.HasCustomOptions, () =>
    {
      RuleFor(q => q.Options)
        .NotEmpty()
        .WithMessage(CreateQuestionRequestValidatorResource.QuestionOption);
    });

    RuleForEach(q => q.Options)
      .Must(o => !o.IsCustom)
      .WithMessage(CreateQuestionRequestValidatorResource.OptionCustom)
      .Must(o => o.Content.Length < 301)
      .WithMessage(CreateQuestionRequestValidatorResource.OptionLong);
  }
}
