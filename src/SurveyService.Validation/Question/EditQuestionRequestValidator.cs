using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Validation.Question;

public class EditQuestionRequestValidator : ExtendedEditRequestValidator<DbQuestion, EditQuestionRequest>, IEditQuestionRequestValidator
{
  private void HandleInternalPropertyValidation(
     Operation<EditQuestionRequest> requestedOperation,
     CustomContext context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    #region paths

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditQuestionRequest.Content),
        nameof(EditQuestionRequest.IsAnonymous),
        nameof(EditQuestionRequest.IsRevoteAvailable),
        nameof(EditQuestionRequest.IsObligatory),
        nameof(EditQuestionRequest.IsPrivate),
        nameof(EditQuestionRequest.HasMultipleChoice),
        nameof(EditQuestionRequest.Deadline),
        nameof(EditQuestionRequest.HasRealTimeResult),
        nameof(EditQuestionRequest.IsActive)
      });

    AddСorrectOperations(nameof(EditQuestionRequest.Content), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.IsAnonymous), new () { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.IsRevoteAvailable), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.IsObligatory), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.IsPrivate), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.HasMultipleChoice), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.Deadline), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.HasRealTimeResult), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditQuestionRequest.IsActive), new() { OperationType.Replace });

    #endregion

    #region content

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Content cannot be empty." },
        { x => x.value.ToString().Trim().Length < 301, "Content is too long." }
      },
      CascadeMode.Stop);

    #endregion

    #region deadline

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.Deadline),
      x => x == OperationType.Replace,
      new()
      {
        { x => DateTime.TryParse(x.value?.ToString(), out DateTime result)
          || (x.value is null),
          "Invalid deadline." },
        { x => !DateTime.TryParse(x.value?.ToString(), out DateTime result)
          || result > DateTime.UtcNow.AddDays(1).AddSeconds(2),
          "The deadline must be at least 24 hours from now." }
      });

    #endregion

    #region is anonymous

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.IsAnonymous),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question is anonymous format"}
      });

    #endregion

    #region is revote available

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.IsRevoteAvailable),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question is revote available format"}
      });

    #endregion

    #region is obligatory

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.IsObligatory),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question is obligatory format"}
      });

    #endregion

    #region is private

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.IsPrivate),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question is private format"}
      });

    #endregion

    #region has multiple choice

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.HasMultipleChoice),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question has multiple choice format"}
      });

    #endregion

    #region has realtime result

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.HasRealTimeResult),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question has realtime result format"}
      });

    #endregion

    #region is active

    AddFailureForPropertyIf(
      nameof(EditQuestionRequest.IsActive),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect question is active format"}
      });

    #endregion
  }

  public EditQuestionRequestValidator()
  {
    RuleForEach(x => x.Item2.Operations)
      .Custom(HandleInternalPropertyValidation);

    When(x => x.Item1.Options
      .Where(option => option.IsActive)
      .Select(option => option.UsersAnswers.FirstOrDefault())
      .ToList()
      .Any()
      && x.Item1.IsAnonymous == true,
      () =>
      {
        RuleFor(x => x.Item2)
        .Must(x =>
        {
          Operation isAnonymousOperation = x.Operations
            .Where(operation => operation.path.EndsWith(nameof(EditQuestionRequest.IsAnonymous), StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

          return !bool.TryParse(isAnonymousOperation?.value?.ToString(), out bool res)
            || res;
        })
        .WithMessage("You cannot make a question non-anonymous if it has already been answered.");
      });
  }
}
