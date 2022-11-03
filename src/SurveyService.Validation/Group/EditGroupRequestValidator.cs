using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Validation.Group;

public class EditGroupRequestValidator : BaseEditRequestValidator<EditGroupRequest>, IEditGroupRequestValidator
{
  private void HandleInternalPropertyValidation(Operation<EditGroupRequest> requestedOperation, CustomContext context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    #region paths

    AddСorrectPaths(
        new List<string>
        {
          nameof(EditGroupRequest.Subject),
          nameof(EditGroupRequest.Description),
          nameof(EditGroupRequest.IsActive)
        });

    AddСorrectOperations(nameof(EditGroupRequest.Subject), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditGroupRequest.Description), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditGroupRequest.IsActive), new() { OperationType.Replace });

    #endregion

    #region subject

    AddFailureForPropertyIf(
      nameof(EditGroupRequest.Subject),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrWhiteSpace(x.value?.ToString()), "Subject cannot be empty." },
        { x => x.value.ToString().Trim().Length < 151, "Subject is too long." }
      },
      CascadeMode.Stop);

    #endregion

    #region description

    AddFailureForPropertyIf(
      nameof(EditGroupRequest.Subject),
      x => x == OperationType.Replace,
      new()
      {
        { x => x.value.ToString().Trim().Length < 501, "Description is too long." }
      },
      CascadeMode.Stop);

    #endregion

    #region is active

    AddFailureForPropertyIf(
      nameof(EditGroupRequest.IsActive),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => bool.TryParse(x.value?.ToString(), out bool _),
          "Incorrect group is active format"}
      });

    #endregion
  }

  public EditGroupRequestValidator()
  {
    RuleForEach(x => x.Operations)
      .Custom(HandleInternalPropertyValidation);
  }
}

