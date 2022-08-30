using FluentValidation;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;

namespace LT.DigitalOffice.SurveyService.Validation.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>, ICreateGroupRequestValidator
{
  public CreateGroupRequestValidator()
  {
    
  }
}