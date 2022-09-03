using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Validation.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>, ICreateGroupRequestValidator
{
  public CreateGroupRequestValidator(
    ICreateQuestionRequestValidator createQuestionRequestValidator)
  {
    RuleFor(group => group.Subject)
      .MaximumLength(150)
      .WithMessage("Subject should not exceed maximum length of 150 symbols.");

    When(group => !string.IsNullOrEmpty(group.Description), () =>
    {
      RuleFor(group => group.Description)
        .MaximumLength(500)
        .WithMessage("Description should not exceed maximum length of 500 symbols.");
    });
    
    RuleFor(group => group.Questions)
      .Cascade(CascadeMode.Stop)
      .Must(questions => questions.Any())
      .WithMessage("At least 1 question in group is required.")
      .ForEach(question => question
        .SetValidator(createQuestionRequestValidator));
  }
}
