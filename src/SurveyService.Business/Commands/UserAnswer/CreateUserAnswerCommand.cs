using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using LT.DigitalOffice.SurveyService.Validation.UserAnswer.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer;

public class CreateUserAnswerCommand : ICreateUserAnswerCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ICreateUserAnswerRequestValidator _validator;
  private readonly IUserAnswerRepository _userAnswerRepository;
  private readonly IDbUserAnswerMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public CreateUserAnswerCommand(
    IHttpContextAccessor httpContextAccessor,
    ICreateUserAnswerRequestValidator validator,
    IUserAnswerRepository userAnswerRepository,
    IDbUserAnswerMapper mapper,
    IResponseCreator responseCreator)
  {
    _httpContextAccessor = httpContextAccessor;
    _validator = validator;
    _userAnswerRepository = userAnswerRepository;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateUserAnswerRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
    }

    OperationResultResponse<Guid?> response = new(
      body: await _userAnswerRepository.CreateAsync(_mapper.Map(request)));

    if (!response.Body.HasValue)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
    }

    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}
