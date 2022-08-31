using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System.Data;

namespace LT.DigitalOffice.SurveyService.Validation.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>, ICreateGroupRequestValidator
{
  public CreateGroupRequestValidator(
    ICreateQuestionRequestValidator createQuestionRequestValidator)
  {
    RuleFor(group => group.Subject)
      .Cascade(CascadeMode.Stop)
      .NotEmpty()
      .WithMessage("Subject can't be empty")
      .MaximumLength(150)
      .WithMessage("Subject should not exceed maximum length of 150 symbols.");

    RuleFor(group => group.Description)
      .MaximumLength(500)
      .WithMessage("Description should not exceed maximum length of 500 symbols.");

    RuleFor(group => group.Questions)
      .NotEmpty()
      .WithMessage("Group should contain at least 1 question.")
      .ForEach(question => question.SetValidator(createQuestionRequestValidator));
  }
}