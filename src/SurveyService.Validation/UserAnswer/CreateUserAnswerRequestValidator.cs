using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Validation.UserAnswer;

public class CreateUserAnswerRequestValidator : AbstractValidator<(CreateUserAnswerRequest, List<DbOption>)>, ICreateUserAnswerRequestValidator
{
  public CreateUserAnswerRequestValidator(
    IHttpContextAccessor _httpContextAccessor)
  {

    RuleFor(request => request.Item1.OptionIds)
      .Must(options => options.Any())
      .WithMessage("There should be at least one option in request.");

    When(x => x.Item2 is not null, () =>
    {
      RuleFor(pair => new
      {
        requestOptions = pair.Item1.OptionIds,
        dbOptions = pair.Item2
      })
        .Cascade(CascadeMode.Stop)
        .Must(x => x.dbOptions.Where(o => o.IsActive).ToList().Count == x.requestOptions.Count)
        .WithMessage("Some options don't exist.")
        .Must(x =>
        {
          if (x.dbOptions.Select(o => o.Question.GroupId).Where(id => id is null).Count() > 0)
          {
            Guid questionId = x.dbOptions.First().QuestionId;
            return x.dbOptions.TrueForAll(o => o.QuestionId == questionId);
          }

          return x.dbOptions.Select(o => o.QuestionId).Distinct().Count() == 1
            || x.dbOptions.Select(o => o.Question.GroupId).Distinct().Count() == 1;
        })
        .WithMessage("Answer must contain options from one question or one group of questions.")
        .Must(x => x.dbOptions.Select(o => o.Question).ToList()
          .TrueForAll(question => question.Deadline is null || question.Deadline > DateTime.UtcNow))
        .WithMessage("Deadline has expired.")
        .Must(x => x.dbOptions.GroupBy(option => option.QuestionId)
          .ToDictionary(qo => qo.Key, qo => qo.ToList()).Values.ToList()
          .TrueForAll(options => options.Count == 1 || options.First().Question.HasMultipleChoice))
        .WithMessage("Several answers are given, to a question that requires one.")
        .Must(x => x.dbOptions.GroupBy(o => o.QuestionId).Select(g => g.First()).ToList()
          .TrueForAll(o => !o.Question.Options
            .SelectMany(o => o.UsersAnswers)
            .Select(answer => answer.UserId).ToList()
            .Contains(_httpContextAccessor.HttpContext.GetUserId())))
        .WithMessage("Trying to answer twice in a question.")
        .Must(x => x.dbOptions.First().Question is null 
          || !x.dbOptions.First().Question.Group.Questions.Where(q => q.IsObligatory)
            .Select(q => q.Id).ToList().Except(x.dbOptions.Select(o => o.QuestionId).Distinct().ToList()).Any())
        .WithMessage("Not all required questions from the group were answered.")
        ;
    });
  }
}
