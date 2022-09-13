using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Validation.UserAnswer;

public class CreateUserAnswerRequestValidator : AbstractValidator<CreateUserAnswerRequest>, ICreateUserAnswerRequestValidator
{
  public CreateUserAnswerRequestValidator(
    IOptionRepository _optionRepository,
    IHttpContextAccessor _httpContextAccessor)
  {

    RuleFor(request => new
    {
      optionsIds = request.OptionIds,
      options = _optionRepository.GetByIdsAsync(request.OptionIds).Result
    })
      .Cascade(CascadeMode.Stop)
      .Must(x => x.optionsIds.Any())
      .WithMessage("There are no optionIds.")
      .Must(x => x.optionsIds.Count == x.options.Count)
      .WithMessage("Some option ids don't exist.")
      .Must(x => x.options.Select(o => o.Question).Distinct().Count() == 1
        || x.options.Select(o => o.Question).Distinct().Select(q => q.GroupId).Distinct().Count() == 1)
      .WithMessage("There are questions from different groups.")
      .Must(x => x.options.Select(o => o.Question).Distinct().ToList()
        .TrueForAll(question => question.Deadline is null
          || DateTime.Compare((DateTime)question.Deadline, DateTime.UtcNow) > 0))
      .WithMessage("Deadline has expired.")
      .Must(x => x.options.GroupBy(option => option.QuestionId)
        .ToDictionary(qo => qo.Key, qo => qo.ToList()).Values.ToList()
        .TrueForAll(options => options.Count == 1 || options.First().Question.HasMultipleChoice))
      .WithMessage("Several answers are given, to a question that requires one.")
      .Must(x => x.options.TrueForAll(o => o.Question.IsRevoteAvailable
        || !o.UsersAnswers.Select(answer => answer.UserId).ToList()
          .Contains(_httpContextAccessor.HttpContext.GetUserId())))
      .WithMessage("Trying to answer twice in a question that doesn't allow it.");
  }
}
