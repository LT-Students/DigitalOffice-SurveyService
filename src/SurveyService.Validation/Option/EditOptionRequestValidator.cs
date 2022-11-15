using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Validation.Option;

public class EditOptionRequestValidator : BaseEditRequestValidator<EditOptionRequest>,  IEditOptionRequestValidator
{
  private void HandleInternalPropertyValidation(
    Operation<EditOptionRequest> requestedOperation,
    CustomContext context)
  {
    RequestedOperation = requestedOperation;
    Context = context;
    
    #region paths
    
    AddСorrectPaths(
      new List<string> {
        nameof(EditOptionRequest.Content),
        nameof(EditOptionRequest.IsActive)
      });
    
    AddСorrectOperations(nameof(EditOptionRequest.Content), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditOptionRequest.IsActive), new() { OperationType.Replace });
    
    #endregion

    #region content

    AddFailureForPropertyIf(
      nameof(EditOptionRequest.Content),
      x => x == OperationType.Replace,
      new Dictionary<Func<Operation<EditOptionRequest>, bool>, string> {
        {x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Content must not be null or empty"},
        {x => x.value.ToString().Length < 301, "Content lenght must be less than 300 symbols."}
      },
      CascadeMode.Stop);

    #endregion

    #region isActive

    AddFailureForPropertyIf(
      nameof(EditOptionRequest.IsActive),
      x => x == OperationType.Replace,
      new Dictionary<Func<Operation<EditOptionRequest>, bool>, string> {
        {x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect value format for IsActive"}
      });

    #endregion
  }

  public EditOptionRequestValidator()
  {
    RuleForEach(x => x.Operations)
      .Custom(HandleInternalPropertyValidation);
  }
}
