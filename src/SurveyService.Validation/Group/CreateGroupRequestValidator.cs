using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.SurveyService.Validation.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>, ICreateGroupRequestValidator
{
  public CreateGroupRequestValidator(
    ICreateGroupQuestionRequestValidator createGroupQuestionRequestValidator)
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
      .NotEmpty()
      .WithMessage("Group must contain at least 1 question.");
    
    When(groupQuestion => groupQuestion.Deadline.HasValue, () =>
    {
      RuleFor(q => q.Deadline.Value)
        .Must(d => d > DateTime.UtcNow.AddDays(1).AddSeconds(2))
        .WithMessage("The deadline must be at least 24 hours from now.");
    }).Otherwise(() =>
    {
      RuleFor(q => q.HasRealTimeResult)
        .Must(realTime => realTime);
    });

    RuleForEach(group => group.Questions)
      .SetValidator(createGroupQuestionRequestValidator);
  }
}
