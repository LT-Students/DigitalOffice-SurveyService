using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.SurveyService.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option.Create;

public class CreateOptionRequestValidator : AbstractValidator<(CreateOptionRequest request, bool? hasCustomOptions)>
{
  public CreateOptionRequestValidator(SurveyServiceDbContext dbContext)
  {
    When(x => !x.hasCustomOptions.HasValue, () =>
    {
      RuleFor(x => x.request.QuestionId)
        .MustAsync(async (x, _) => await dbContext.Questions.AnyAsync(q => q.Id == x))
        .WithMessage("The question id doesn't exist.");

      When(x => x.request.IsCustom, () =>
      {
        RuleFor(x => x.request.QuestionId)
          .MustAsync(async (x, _) => (await dbContext.Questions.FirstOrDefaultAsync(q => q.Id == x))!.HasCustomOptions)
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
