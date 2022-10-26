using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class EditQuestionCommand : IEditQuestionCommand
{
  private readonly IQuestionRepository _questionRepository;
  private readonly IOptionRepository _optionRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IResponseCreator _responseCreator;
  private readonly IAccessValidator _accessValidator;
  private readonly IEditQuestionRequestValidator _validator;
  private readonly IPatchDbQuestionMapper _mapper;


  public EditQuestionCommand(
    IQuestionRepository questionRepository,
    IOptionRepository optionRepository,
    IPatchDbQuestionMapper mapper,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor,
    IResponseCreator responseCreator,
    IEditQuestionRequestValidator validator)
  {
    _questionRepository = questionRepository;
    _optionRepository = optionRepository;
    _mapper = mapper;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
    _responseCreator = responseCreator;
    _validator = validator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid questioId, JsonPatchDocument<EditQuestionRequest> request)
  {
    Guid requestSenderId = _httpContextAccessor.HttpContext.GetUserId();
    bool isAdmin = await _accessValidator.IsAdminAsync(requestSenderId);

    DbQuestion question = await _questionRepository.GetQuestionWithAnswersAsync(questioId);

    if (!isAdmin && requestSenderId != question.CreatedBy)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync((question, request));

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>
       (HttpStatusCode.BadRequest, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
    }

    string isActiveStr = request.Operations
      .Where(operation => operation.path.Equals("/IsActive"))
      .FirstOrDefault()?
      .value.ToString();

    if ((isActiveStr is null) || isActiveStr.Equals("false"))
    {
      await _optionRepository.EditOptionsActivityAsync(question.Options);
    }

    return new OperationResultResponse<bool>(
      body: await _questionRepository.EditAsync(_mapper.Map(request), question));
  }
}
