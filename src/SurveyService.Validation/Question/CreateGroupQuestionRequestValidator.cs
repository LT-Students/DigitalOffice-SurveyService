using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Resources;
using System.Globalization;
using System.Threading;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateGroupQuestionRequestValidator : AbstractValidator<CreateGroupQuestionRequest>, ICreateGroupQuestionRequestValidator
{
  public CreateGroupQuestionRequestValidator()
  {
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");

    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage(CreateQuestionRequestValidatorResource.ContentTooLong);

    When(q => !q.HasCustomOptions, () =>
    {
      RuleFor(q => q.Options)
        .NotEmpty()
        .WithMessage(CreateQuestionRequestValidatorResource.QuestionWithNoOption);
    });

    RuleForEach(q => q.Options)
      .Must(o => !o.IsCustom)
      .WithMessage(CreateQuestionRequestValidatorResource.OptionCustom)
      .Must(o => o.Content.Length < 301)
      .WithMessage(CreateQuestionRequestValidatorResource.OptionTooLong);
  }
}
