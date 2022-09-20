using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer;

public class CreateUserAnswerCommand : ICreateUserAnswerCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ICreateUserAnswerRequestValidator _validator;
  private readonly IUserAnswerRepository _userAnswerRepository;
  private readonly IOptionRepository _optionRepository;
  private readonly IDbUserAnswerMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public CreateUserAnswerCommand(
    IHttpContextAccessor httpContextAccessor,
    ICreateUserAnswerRequestValidator validator,
    IUserAnswerRepository userAnswerRepository,
    IOptionRepository optionRepository,
    IDbUserAnswerMapper mapper,
    IResponseCreator responseCreator)
  {
    _httpContextAccessor = httpContextAccessor;
    _validator = validator;
    _userAnswerRepository = userAnswerRepository;
    _optionRepository = optionRepository;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(CreateUserAnswerRequest request)
  {
    request.OptionIds = request.OptionIds.Distinct().ToList();
    ValidationResult validationResult = await _validator.ValidateAsync((request, null));

    if (validationResult.IsValid)
    {
      List<DbOption> options = await _optionRepository.GetByIdsAsync(request.OptionIds);
      validationResult = await _validator.ValidateAsync((request, options));
    }

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
    }

    OperationResultResponse<bool> response = new(
      body: await _userAnswerRepository.CreateAsync(request.OptionIds.Select(_mapper.Map).ToList()));

    if (!response.Body)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
    }

    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}
