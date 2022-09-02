using FluentValidation;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;

namespace LT.DigitalOffice.SurveyService.Validation.UserAnswer;

public class CreateUserAnswerRequestValidator : AbstractValidator<CreateUserAnswerRequest>, ICreateUserAnswerRequestValidator
{
  private readonly IOptionRepository _optionRepository;

  public CreateUserAnswerRequestValidator(
    IOptionRepository optionRepository)
  {
    _optionRepository = optionRepository;

    RuleFor(request => request.OptionId)
      .MustAsync(async (optionId, _) => await _optionRepository.DoesExistAsync(optionId))
      .WithMessage("The option id doesn't exist.");
  }
}
