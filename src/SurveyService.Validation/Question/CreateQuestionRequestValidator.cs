﻿using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using System;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class CreateQuestionRequestValidator : AbstractValidator<CreateSingleQuestionRequest>, ICreateQuestionRequestValidator
{
  public CreateQuestionRequestValidator(
    IGroupRepository _groupRepository
    )
  {
    RuleFor(q => q.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    When(q => q.Deadline.HasValue, () =>
    {
      RuleFor(q => q.Deadline)
        .Must(d => d > DateTime.UtcNow.AddDays(1).AddSeconds(2))
        .WithMessage("The deadline must be at least 24 hours from now.");
    });

    When(q => q.GroupId.HasValue, () =>
    {
      RuleFor(q => q)
        .MustAsync(async (q, _) => (await _groupRepository.GetPropertiesAsync((Guid)q.GroupId)).Deadline == q.Deadline)
        .WithMessage("The deadlines of questions in the group are not equal.");

      RuleFor(q => q)
        .MustAsync(async (q, _) => (await _groupRepository.GetPropertiesAsync((Guid)q.GroupId)).HasRealTimeResult == q.HasRealTimeResult)
        .WithMessage("The conditions for displaying the results of the questions are different.");
    });

    When(q => !q.HasCustomOptions, () =>
    {
      RuleFor(q => q.Options)
        .NotEmpty()
        .WithMessage("This question should have one option at least.");
    });

    RuleForEach(q => q.Options)
      .Must(q => q.Content.Length < 301)
      .WithMessage("Option is too long");
  }
}