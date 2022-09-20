using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class CreateQuestionCommand : ICreateQuestionCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IDbSingleQuestionMapper _mapper;
  private readonly IQuestionRepository _questionRepository;
  private readonly ICreateQuestionRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;

  public CreateQuestionCommand(
    IHttpContextAccessor httpContextAccessor, 
    IDbSingleQuestionMapper mapper, 
    IQuestionRepository questionRepository,
    ICreateQuestionRequestValidator validator,
    IResponseCreator responseCreator)
  {
    _httpContextAccessor = httpContextAccessor;
    _mapper = mapper;
    _questionRepository = questionRepository;
    _validator = validator;
    _responseCreator = responseCreator;
  }
  
  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateSingleQuestionRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(validationFailure => validationFailure.ErrorMessage).ToList());
    }

    OperationResultResponse<Guid?> response = new(
      body: await _questionRepository.CreateAsync(_mapper.Map(request)));

    if (response.Body is null)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
    }

    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}